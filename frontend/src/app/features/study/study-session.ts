import {
  ChangeDetectionStrategy,
  Component,
  computed,
  DestroyRef,
  inject,
  input,
  OnInit,
  signal,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { timer } from 'rxjs';

import { AnswerResult, RatingPreview, ReviewRating, ScenarioCard, ScenarioCategory, StepOption } from '../../core/models/api.models';
import { CATEGORY_LABELS, formatInterval, MASTERY_LABELS, RATING_LABELS } from '../../core/models/labels';
import { ScenarioService } from '../../core/services/scenario.service';
import { AutoHeight } from '../../shared/auto-height';

type Phase = 'loading' | 'load-error' | 'empty' | 'answering' | 'success' | 'failed' | 'complete';
type OptionState = 'idle' | 'pending' | 'correct' | 'wrong' | 'dimmed';

interface ClearedStep {
  prompt: string;
  action: string;
  explanation: string;
}

/** How long the verdict stays on the options before the view moves on. */
const CORRECT_REVEAL_MS = 650;
const WRONG_REVEAL_MS = 1100;
/** Matches .exit-card in styles.css. */
const CARD_EXIT_MS = 220;

const INVERT_ON_HOVER =
  'hover:bg-ink-charcoal hover:text-ink-paper dark:hover:bg-ink-bone dark:hover:text-ink-coal';

const OPTION_CLASSES: Record<OptionState, string> = {
  idle: `rule cursor-pointer ${INVERT_ON_HOVER}`,
  pending: 'rule fill-ink',
  correct: 'rule fill-ink',
  wrong: 'shake border-ink-blood bg-ink-blood text-ink-paper',
  dimmed: 'rule opacity-40',
};

const RATING_CLASSES: Record<ReviewRating, string> = {
  Again: 'hover:bg-ink-blood hover:text-ink-paper',
  Hard: INVERT_ON_HOVER,
  Good: INVERT_ON_HOVER,
  Easy: INVERT_ON_HOVER,
  VeryEasy: INVERT_ON_HOVER,
};

@Component({
  selector: 'app-study-session',
  imports: [AutoHeight, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './study-session.html',
  host: { '(window:keydown)': 'onKeydown($event)' },
})
export class StudySession implements OnInit {
  private readonly scenarios = inject(ScenarioService);
  private readonly destroyRef = inject(DestroyRef);

  /** Bound from the ?category= query parameter. */
  readonly category = input<ScenarioCategory>();

  protected readonly categoryLabels = CATEGORY_LABELS;
  protected readonly masteryLabels = MASTERY_LABELS;
  protected readonly ratingLabels = RATING_LABELS;
  protected readonly formatInterval = formatInterval;
  protected readonly ratingClasses = RATING_CLASSES;

  protected readonly phase = signal<Phase>('loading');
  protected readonly cards = signal<ScenarioCard[]>([]);
  protected readonly cardIndex = signal(0);
  protected readonly stepIndex = signal(0);
  protected readonly selectedOptionId = signal<string | null>(null);
  protected readonly answer = signal<AnswerResult | null>(null);
  protected readonly clearedSteps = signal<ClearedStep[]>([]);
  protected readonly submitting = signal(false);
  protected readonly exiting = signal(false);
  protected readonly error = signal<string | null>(null);
  protected readonly clearedCount = signal(0);
  protected readonly failedCount = signal(0);

  protected readonly card = computed(() => this.cards()[this.cardIndex()]);
  protected readonly step = computed(() => this.card()?.steps[this.stepIndex()]);
  protected readonly selectedOption = computed(() =>
    this.step()?.options.find((option) => option.id === this.selectedOptionId()),
  );

  /** One-item lists, so `@for` can treat a new card or step as a new element to animate. */
  protected readonly visibleCard = computed(() => {
    const card = this.card();
    return card ? [card] : [];
  });
  protected readonly visibleStep = computed(() => {
    const step = this.step();
    return step ? [step] : [];
  });

  protected readonly sessionPercent = computed(() => {
    const finished = this.cardIndex() + (this.phase() === 'complete' ? 1 : 0);
    return this.cards().length ? (finished / this.cards().length) * 100 : 0;
  });

  ngOnInit(): void {
    this.load();
  }

  protected load(): void {
    this.phase.set('loading');
    this.scenarios
      .getQueue(this.category())
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (cards) => {
          this.cards.set(cards);
          this.phase.set(cards.length ? 'answering' : 'empty');
        },
        error: () => this.phase.set('load-error'),
      });
  }

  protected optionState(option: StepOption): OptionState {
    const selectedId = this.selectedOptionId();
    if (!selectedId) {
      return 'idle';
    }

    const answer = this.answer();
    if (!answer) {
      return option.id === selectedId ? 'pending' : 'dimmed';
    }
    if (option.id === answer.correctOptionId) {
      return 'correct';
    }
    return option.id === selectedId ? 'wrong' : 'dimmed';
  }

  protected optionClasses(option: StepOption): string {
    return OPTION_CLASSES[this.optionState(option)];
  }

  protected choose(option: StepOption): void {
    const card = this.card();
    const step = this.step();
    if (!card || !step || this.phase() !== 'answering' || this.selectedOptionId()) {
      return;
    }

    this.error.set(null);
    this.selectedOptionId.set(option.id);

    this.scenarios
      .submitAnswer(card.id, step.id, option.id)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (answer) => {
          this.answer.set(answer);
          this.after(answer.isCorrect ? CORRECT_REVEAL_MS : WRONG_REVEAL_MS, () => this.settle(answer));
        },
        error: () => {
          this.selectedOptionId.set(null);
          this.error.set("That answer couldn't be saved. Please try again.");
        },
      });
  }

  protected rate(preview: RatingPreview): void {
    const card = this.card();
    if (!card || this.phase() !== 'success' || this.submitting()) {
      return;
    }

    this.error.set(null);
    this.submitting.set(true);

    this.scenarios
      .submitReview(card.id, preview.rating)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: () => this.nextCard(),
        error: () => {
          this.submitting.set(false);
          this.error.set("That rating couldn't be saved. Please try again.");
        },
      });
  }

  protected nextCard(): void {
    if (this.exiting()) {
      return;
    }

    this.exiting.set(true);
    this.after(CARD_EXIT_MS, () => {
      const isLast = this.cardIndex() === this.cards().length - 1;
      this.exiting.set(false);
      this.submitting.set(false);

      if (isLast) {
        this.phase.set('complete');
        return;
      }

      this.cardIndex.update((index) => index + 1);
      this.stepIndex.set(0);
      this.clearedSteps.set([]);
      this.resetAnswer();
      this.phase.set('answering');
    });
  }

  protected onKeydown(event: KeyboardEvent): void {
    if (event.ctrlKey || event.metaKey || event.altKey || event.repeat) {
      return;
    }

    const number = Number(event.key);
    const index = Number.isInteger(number) ? number - 1 : -1;

    switch (this.phase()) {
      case 'answering': {
        const option = this.step()?.options[index];
        if (option) {
          this.choose(option);
        }
        break;
      }
      case 'success': {
        const preview = this.answer()?.ratingPreviews?.[index];
        if (preview) {
          this.rate(preview);
        }
        break;
      }
      case 'failed':
        if (event.key === 'Enter') {
          event.preventDefault();
          this.nextCard();
        }
        break;
    }
  }

  private settle(answer: AnswerResult): void {
    if (!answer.isCorrect) {
      this.failedCount.update((count) => count + 1);
      this.phase.set('failed');
      return;
    }

    const step = this.step()!;
    this.clearedSteps.update((steps) => [
      ...steps,
      { prompt: step.prompt, action: answer.correctAction, explanation: answer.stepExplanation },
    ]);

    if (answer.isScenarioComplete) {
      this.clearedCount.update((count) => count + 1);
      this.phase.set('success');
      return;
    }

    this.resetAnswer();
    this.stepIndex.update((index) => index + 1);
  }

  private resetAnswer(): void {
    this.selectedOptionId.set(null);
    this.answer.set(null);
    this.error.set(null);
  }

  private after(ms: number, action: () => void): void {
    timer(ms).pipe(takeUntilDestroyed(this.destroyRef)).subscribe(action);
  }
}
