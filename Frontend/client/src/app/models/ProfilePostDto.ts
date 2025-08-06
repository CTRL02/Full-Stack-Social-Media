import { ProfileCommentDto } from "./ProfileCommentDto";
import { ProfileImpressionDto } from "./ProfileImpressionDto";

export interface ProfilePostDto {
  id: number;
  content: string;
  createdAt: string;
  impressions: ProfileImpressionDto[];
  comments: ProfileCommentDto[];
}
