import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { impressionToggleDto } from '../models/impressionToggleDto';
import { Observable } from 'rxjs';
import { Post } from '../memeber-profile/post/post';
import { ProfilePostDto } from '../models/ProfilePostDto';
import { feedModel } from '../models/feedModel';

@Injectable({
  providedIn: 'root'
})
export class UserActions {
  private baseUrl = 'http://localhost:5080/api/Impressions';

  constructor(private http: HttpClient) { } 

  toggleImpression(dto: impressionToggleDto) {
    return this.http.post<{message: string}>(`${this.baseUrl}/leaveImpression`, dto);
  }
  private apiUrl = 'http://localhost:5080/comment';
  postComment(postId: number, content: string, parentCommentId?: number): Observable<any> {
    let params = new HttpParams()
      .set('Content', content)
      .set('PostId', postId.toString());

    if (parentCommentId) {
      params = params.set('ParentCommentId', parentCommentId.toString());
    }

    return this.http.post(this.apiUrl, {}, { params });
  }

  private urlApi = 'http://127.0.0.1:5000/recommend';

  getRecommendedPosts(username: string|null, top_n: number): Observable<feedModel[]> {
    return this.http.get<feedModel[]>(`${this.urlApi}?username=${username}&top_n=${top_n}`);
  }

  
}
