using AutoMapper;
using socialmedia.DTOs;
using socialmedia.DTOs.userProfileDtos;
using socialmedia.Entities;

namespace socialmedia.Helper
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, activeusersDto>();

            CreateMap<AppUser, userProfileDto>()
                .ForMember(dest => dest.noOfPosts, opt => opt.MapFrom(src => src.Posts.Count))
                .ForMember(dest => dest.noOfFollowers, opt => opt.MapFrom(src => src.Followers.Count))
                .ForMember(dest => dest.noOfFollowing, opt => opt.MapFrom(src => src.Following.Count))
                .ForMember(dest => dest.socialLinks, opt => opt.MapFrom(src => src.SocialLinks.Select(sl => sl.Url).ToList()))
                .ForMember(dest => dest.followers, opt => opt.MapFrom(src => src.Followers.Select(f => f.Follower)))
                .ForMember(dest => dest.following, opt => opt.MapFrom(src => src.Following.Select(f => f.Followee)));

            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.avatar, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());

            CreateMap<CommentDto, Comment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())  
                .ForMember(dest => dest.AppUserId, opt => opt.Ignore())   
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.ParentCommentId, opt => opt.MapFrom(src => src.ParentCommentId));

            CreateMap<ImpressionDto, Impression>()
                .ForMember(dest => dest.Type, opt => opt.Ignore())         
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())   
                .ForMember(dest => dest.AppUserId, opt => opt.Ignore())   
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.CommentId));


            CreateMap<Impression, profileimpressionDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.avatar, opt => opt.MapFrom(src => src.AppUser.avatar));

            CreateMap<Comment, profilecommentDto>()
                .ForMember(dest => dest.username, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.avatar, opt => opt.MapFrom(src => src.AppUser.avatar))
                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies));

            CreateMap<Post, profilepostDto>()
                .ForMember(dest => dest.Comments,
                    opt => opt.MapFrom(src =>
                        src.Comments
                            .Where(c => c.ParentCommentId == null) // filter out replies
                    ));

        }
    }



}

