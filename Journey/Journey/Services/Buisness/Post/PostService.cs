using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Post;
using Journey.Services.Buisness.Post.Data;

namespace Journey.Services.Buisness.Post
{
    public class PostService : IPostService
    {
        private readonly IPostDataService _postDataService;

        private const int PostPageSize = 10;

        public PostService(IPostDataService postDataService)
        {
            _postDataService = postDataService;
            RefreshPosts = true;
        }

        public bool RefreshPosts { get; set; }

        public event PostStatusChangedEventHandler PostStatusChangedEventHandler;

        public async Task<Models.Post.Post> AddPostAsync(Models.Post.Post post, List<string> images)
        {
            try
            {
                PostStatusChanged(post, PostStatus.InProgress);
                Models.Post.Post postbase = await _postDataService.AddPostAsync(post, images);
                if (postbase != null)
                    PostStatusChanged(postbase, PostStatus.Added);
                return postbase;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> DeletePostAsync(PostBase post)
        {
            try
            {
                PostStatusChanged(post, PostStatus.InProgress);
                bool status = await _postDataService.DeletePostAsync(post);
                if (status)
                    PostStatusChanged(post, PostStatus.Deleted);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public async Task<List<PostBase>> GetPostsAsync(int page, bool sync = false)
        {
            try
            {
                List<Models.Post.Post> postList =
                    await _postDataService.GetPostsAsync(page, PostPageSize, sync);

                if (postList == null)
                    return null;

                var posts = new List<PostBase>();

                foreach (Models.Post.Post post in postList)
                    posts.Add(post);

                RefreshPosts = false;
                return posts;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> LikeAsync(PostBase post)
        {
            try
            {
                return await _postDataService.LikeAsync(post);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> ShareAsync(PostBase post)
        {
            try
            {
                return await _postDataService.ShareAsync(post);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void PostStatusChanged(PostBase post, PostStatus status)
        {
            try
            {
                PostStatusChangedEventHandler?.Invoke(this, new PostStatusChangedArgs { Post = post, Status = status });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}