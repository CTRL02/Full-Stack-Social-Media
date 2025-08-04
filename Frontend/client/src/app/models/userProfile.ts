import { allUsersModel } from "./allusersModel";
import { PostModel } from "./postModel";
import { ProfilePostDto } from "./ProfilePostDto";

export interface UserProfile {
  username: string;
  avatar: string;
  bio: string;
  title: string;
  noOfPosts: number;
  noOfFollowers: number;
  noOfFollowing: number;
  followers: allUsersModel[];
  following: allUsersModel[];
  socialLinks: string[];
  posts: ProfilePostDto[];
}
