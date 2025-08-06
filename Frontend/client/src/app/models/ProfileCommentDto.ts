import { ProfileImpressionDto } from "./ProfileImpressionDto";

export interface ProfileCommentDto {
  id: number;
  content: string;
  createdAt: string;
  username: string;
  avatar: string;
  impressions: ProfileImpressionDto[];
  replies: ProfileCommentDto[];
}
