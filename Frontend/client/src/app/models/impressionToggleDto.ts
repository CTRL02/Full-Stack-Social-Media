// src/app/models/impression-dto.ts
export interface impressionToggleDto {
  type: string;         // "Like", "Love", etc.
  postId?: number;
  commentId?: number;
}
