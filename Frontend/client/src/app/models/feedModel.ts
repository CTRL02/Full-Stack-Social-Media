import { ProfileCommentDto } from "./ProfileCommentDto";
import { ProfileImpressionDto } from "./ProfileImpressionDto";

export interface feedModel {
  UserName: string;
  avatar: string;
  Content: string;
  CreatedAt: Date;
  Id: number;
  impressions: ProfileImpressionDto[];
  comments:ProfileCommentDto[];


}
