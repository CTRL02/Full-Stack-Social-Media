import { allUsersModel } from "./allusersModel";

export interface userProfile {
  username: string;
  avatar: string;
  bio: string;
  title: string;
  noOfPosts: number;
  noOfFollowers: number;
  noOfFollowing: number;
  followers: allUsersModel[];     // List of users following this user
  following: allUsersModel[];     // List of users this user follows
  socialLinks: string[];       // URLs to social profiles
}
