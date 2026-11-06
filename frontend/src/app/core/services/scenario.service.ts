import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  AnswerResult,
  ReviewRating,
  ScenarioCard,
  ScenarioCategory,
  ScenarioSummary,
  SubmitAnswerRequest,
  SubmitReviewRequest,
  UserProgress,
} from '../models/api.models';

@Injectable({ providedIn: 'root' })
export class ScenarioService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api/scenarios';

  getAll(): Observable<ScenarioSummary[]> {
    return this.http.get<ScenarioSummary[]>(this.baseUrl);
  }

  getById(id: string): Observable<ScenarioCard> {
    return this.http.get<ScenarioCard>(`${this.baseUrl}/${id}`);
  }

  getQueue(category?: ScenarioCategory): Observable<ScenarioCard[]> {
    const params = category ? new HttpParams().set('category', category) : undefined;
    return this.http.get<ScenarioCard[]>(`${this.baseUrl}/queue`, { params });
  }

  submitAnswer(cardId: string, stepId: string, selectedOptionId: string): Observable<AnswerResult> {
    const body: SubmitAnswerRequest = { selectedOptionId };
    return this.http.post<AnswerResult>(`${this.baseUrl}/${cardId}/steps/${stepId}/answer`, body);
  }

  submitReview(cardId: string, rating: ReviewRating): Observable<UserProgress> {
    const body: SubmitReviewRequest = { rating };
    return this.http.post<UserProgress>(`${this.baseUrl}/${cardId}/review`, body);
  }
}
