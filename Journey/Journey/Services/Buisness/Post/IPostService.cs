using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.Post
{
    public interface IPostService
    {
        bool RefreshPosts { get; set; }

        Task<Models.Post.Post> AddPostAsync(Models.Post.Post post, List<string> images);

        Task<bool> DeletePostAsync(PostBase post);

        Task<List<PostBase>> GetPostsAsync(int pageNo, bool sync = false);

        Task<bool> LikeAsync(PostBase post);

        void PostStatusChanged(PostBase post, PostStatus status);
        event PostStatusChangedEventHandler PostStatusChangedEventHandler;

        Task<bool> ShareAsync(PostBase post);
    }

    public delegate void PostStatusChangedEventHandler(object sender, PostStatusChangedArgs e);

    public class PostStatusChangedArgs : EventArgs
    {
        public PostBase Post { get; set; }

        public PostStatus Status { get; set; }
    }
}