// Mirrors backend/SleepyDuck.Api/Dtos/ApiDtos.cs and Models/Enums.cs.

export type ScenarioCategory =
  | 'EmailDelivery'
  | 'UserLicenseManagement'
  | 'GroupManagement'
  | 'MailFlowRules';

export type MasteryLevel = 'New' | 'Learning' | 'Reviewing' | 'Mastered';

export type ReviewRating = 'Again' | 'Hard' | 'Good' | 'Easy';

export interface StepOption {
  id: string;
  text: string;
}

export interface SequentialStep {
  id: string;
  order: number;
  prompt: string;
  options: StepOption[];
}

export interface UserProgress {
  scenarioCardId: string;
  timesStudied: number;
  masteryLevel: MasteryLevel;
  lastStudiedDate: string | null;
  nextReviewDate: string | null;
  easeFactor: number;
  intervalDays: number;
  repetitions: number;
  lapses: number;
}

export interface ScenarioSummary {
  id: string;
  title: string;
  category: ScenarioCategory;
  stepCount: number;
  progress: UserProgress | null;
}

export interface ScenarioCard {
  id: string;
  title: string;
  category: ScenarioCategory;
  problemStatement: string;
  steps: SequentialStep[];
  progress: UserProgress | null;
}

export interface SubmitAnswerRequest {
  selectedOptionId: string;
}

export interface RatingPreview {
  rating: ReviewRating;
  intervalDays: number;
  nextReviewDate: string;
}

export interface AnswerResult {
  isCorrect: boolean;
  correctOptionId: string;
  correctAction: string;
  stepExplanation: string;
  distractorExplanation: string | null;
  isScenarioComplete: boolean;
  technicalExplanation: string | null;
  ratingPreviews: RatingPreview[] | null;
  progress: UserProgress | null;
}

export interface SubmitReviewRequest {
  rating: ReviewRating;
}

export interface CategoryStats {
  category: ScenarioCategory;
  totalCards: number;
  cardsToReviewToday: number;
  newCards: number;
  masteredCards: number;
}

export interface DashboardStats {
  totalCards: number;
  cardsToReviewToday: number;
  newCards: number;
  masteredCards: number;
  nextReviewDate: string | null;
  categories: CategoryStats[];
}
