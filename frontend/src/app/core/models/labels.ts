import { MasteryLevel, ScenarioCategory } from './api.models';

export const CATEGORY_LABELS: Record<ScenarioCategory, string> = {
  EmailDelivery: 'Email Delivery',
  UserLicenseManagement: 'Users & Licenses',
  GroupManagement: 'Group Management',
  MailFlowRules: 'Mail Flow Rules',
};

export const MASTERY_LABELS: Record<MasteryLevel, string> = {
  New: 'New',
  Learning: 'Learning',
  Reviewing: 'Reviewing',
  Mastered: 'Mastered',
};

/** Formats an SRS interval for a rating button. Zero means the short relearn delay. */
export function formatInterval(days: number): string {
  if (days === 0) {
    return '<10 min';
  }
  if (days === 1) {
    return '1 day';
  }
  if (days < 30) {
    return `${days} days`;
  }
  if (days < 365) {
    return `${(days / 30).toFixed(1)} mo`;
  }
  return '1 yr';
}
