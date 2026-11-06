import { MasteryLevel, ReviewRating, ScenarioCategory } from './api.models';

export const CATEGORY_LABELS: Record<ScenarioCategory, string> = {
  EmailDelivery: 'Email Delivery',
  UserLicenseManagement: 'Users & Licenses',
  GroupManagement: 'Group Management',
  MailFlowRules: 'Mail Flow Rules',
  VoipSupport: 'VOIP Support',
  NetworkTroubleshooting: 'Network Troubleshooting',
  MacNetworkIssues: 'Mac Network Issues',
};

export const MASTERY_LABELS: Record<MasteryLevel, string> = {
  New: 'New',
  Learning: 'Learning',
  Reviewing: 'Reviewing',
  Mastered: 'Mastered',
};

export const RATING_LABELS: Record<ReviewRating, string> = {
  Again: 'Again',
  Hard: 'Hard',
  Good: 'Good',
  Easy: 'Easy',
  VeryEasy: 'Very Easy',
};

function plural(count: number, unit: string): string {
  return `${count} ${unit}${count === 1 ? '' : 's'}`;
}

/**
 * Formats an SRS interval for a rating button, always in whole units of the largest
 * sensible size: 1/24 becomes "1 hour", 0.5 becomes "12 hours". Zero means the short
 * relearn delay.
 */
export function formatInterval(days: number): string {
  if (days <= 0) {
    return '<10 min';
  }

  const hours = Math.round(days * 24);
  if (hours < 1) {
    return `${Math.max(1, Math.round(days * 24 * 60))} min`;
  }
  if (hours < 24) {
    return plural(hours, 'hour');
  }

  const wholeDays = Math.round(days);
  if (wholeDays < 30) {
    return plural(wholeDays, 'day');
  }
  if (wholeDays < 365) {
    return plural(Math.round(wholeDays / 30), 'month');
  }
  return '1 year';
}
