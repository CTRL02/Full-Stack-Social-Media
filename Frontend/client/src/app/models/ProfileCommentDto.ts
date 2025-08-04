import { ProfileImpressionDto } from "./ProfileImpressionDto";

export interface ProfileCommentDto {
  content: string;
  createdAt: string;
  username: string;
  avatar: string;
  impressions: ProfileImpressionDto[];
  replies: ProfileCommentDto[];
}
