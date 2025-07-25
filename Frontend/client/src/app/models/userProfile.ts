import { allUsersModel } from "./allusersModel";
import { PostModel } from "./postModel";

export interface userProfile {
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
  posts: PostModel[];
}
