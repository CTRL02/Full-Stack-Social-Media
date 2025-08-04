import { ProfileCommentDto } from "./ProfileCommentDto";
import { ProfileImpressionDto } from "./ProfileImpressionDto";

export interface ProfilePostDto {
  content: string;
  createdAt: string;
  impressions: ProfileImpressionDto[];
  comments: ProfileCommentDto[];
}
