import { DatePipe } from '@angular/common';
import { ChangeDetectionStrategy, Component, computed, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';

import { CategoryStats, DashboardStats } from '../../core/models/api.models';
import { CATEGORY_LABELS } from '../../core/models/labels';
import { DashboardService } from '../../core/services/dashboard.service';

/** Matches the backend's default newLimit for the study queue. */
const NEW_CARDS_PER_SESSION = 10;

@Component({
  selector: 'app-dashboard',
  imports: [DatePipe, RouterLink],
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './dashboard.html',
})
export class Dashboard implements OnInit {
  private readonly dashboard = inject(DashboardService);
  private readonly destroyRef = inject(DestroyRef);

  protected readonly categoryLabels = CATEGORY_LABELS;
  protected readonly stats = signal<DashboardStats | null>(null);
  protected readonly failed = signal(false);

  protected readonly sessionSize = computed(() => {
    const stats = this.stats();
    return stats ? stats.cardsToReviewToday + Math.min(stats.newCards, NEW_CARDS_PER_SESSION) : 0;
  });

  ngOnInit(): void {
    this.load();
  }

  protected load(): void {
    this.failed.set(false);
    this.dashboard
      .getStats()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (stats) => this.stats.set(stats),
        error: () => this.failed.set(true),
      });
  }

  protected masteredPercent(category: CategoryStats): number {
    return Math.round((category.masteredCards / category.totalCards) * 100);
  }

  protected studiedPercent(category: CategoryStats): number {
    return Math.round(((category.totalCards - category.newCards) / category.totalCards) * 100);
  }

  protected hasWork(category: CategoryStats): boolean {
    return category.cardsToReviewToday + category.newCards > 0;
  }
}
