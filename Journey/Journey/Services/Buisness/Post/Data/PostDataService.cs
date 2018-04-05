using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Post;
using Journey.Services.Azure;
using Journey.Services.Buisness.Post.Dto;
using Journey.Services.Buisness.Post.Translators;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Post.Data
{
    public class PostDataService : IPostDataService
    {
        private readonly IMobileServiceTable<AzurePost> _azurePost;
        private readonly MobileServiceClient _client;

        public PostDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _azurePost = _client.GetTable<AzurePost>();
        }

        public async Task<Models.Post.Post> AddPostAsync(Models.Post.Post post, List<string> images)
        {
            try
            {
                if (post == null)
                    return null;

                AzurePost postDto = PostDataTranslators.TranslatePost(post, _client.CurrentUser.UserId, images);
                postDto.Liked = null;
                await _azurePost.InsertAsync(postDto);

                await SyncPostAsync(post.Challenge);

                post = PostDataTranslators.TranslatePost(postDto);
                return post;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<bool> DeletePostAsync(PostBase post)
        {
            try
            {
                if (post == null)
                    return false;

                await _azurePost.DeleteAsync(new AzurePost {Id = post.Id});

                await SyncPostAsync(post.Challenge);

                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<List<Models.Post.Post>> GetPostsAsync(string challengeId, int page, int size,
            bool sync = false)
        {
            try
            {
                List<AzurePost> posts;
                string api = string.Format("post?size={0}&page={1}&challenge={2}", size, page, challengeId);
                posts = await _client.InvokeApiAsync<List<AzurePost>>(api, HttpMethod.Get, null);
                //if (sync)
                //{
                //    posts = await SyncPostAsync(challengeId, page, size);
                //}
                //if (posts == null || posts.Count == 0)
                //{
                //    posts = await this.azurePost.Where(po => po.Challenge == challengeId).ToListAsync();
                //}
                //if (posts == null || posts.Count == 0)
                //{
                //    posts = await SyncPostAsync(challengeId, page, size);
                //}
                if (posts == null || posts.Count == 0)
                    return null;
                List<Models.Post.Post> postsDTo = PostDataTranslators.TranslatePosts(posts);
                return postsDTo;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<bool> LikeAsync(PostBase post)
        {
            try
            {
                var api = "post";
                var param = new Dictionary<string, string>();
                if (post.Liked)
                    param.Add("action", post.Id + "," + "like");
                else
                    param.Add("action", post.Id + "," + "unlike");
                bool success = await _client.InvokeApiAsync<bool>(api, HttpMethod.Put, param);
                return success;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<bool> ShareAsync(PostBase post)
        {
            try
            {
                var api = "post";
                var param = new Dictionary<string, string>();
                param.Add("action", post.Id + "," + "share");
                bool success = await _client.InvokeApiAsync<bool>(api, HttpMethod.Put, param);
                return success;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }


        public async Task<List<AzurePost>> SyncPostAsync(string challengeId, int page = 0, int size = 10) => null;
    }
}