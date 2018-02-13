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
        //public enum Unit
        //{
        //    Year,
        //    KG,
        //    CM,
        //    KCAL,
        //    Steps
        //}

        private const int PostPageSize = 10;

        private readonly IPostDataService _postDataService;

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
                var postbase = await _postDataService.AddPostAsync(post, images);
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
                var status = await _postDataService.DeletePostAsync(post);
                if (status)
                    PostStatusChanged(post, PostStatus.Deleted);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public async Task<List<PostBase>> GetPostsAsync(int page, string challengeId = null, bool sync = false)
        {
            try
            {
                var postList =
                    await _postDataService.GetPostsAsync(challengeId, page, PostPageSize, sync);

                if (postList == null || postList.Count == 0)
                    return null;

                var posts = new List<PostBase>();

                foreach (var post in postList)
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
            PostStatusChangedEventHandler?.Invoke(this, new PostStatusChangedArgs {Post = post, Status = status});
        }
    }
}