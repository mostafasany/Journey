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

                var postDto = PostDataTranslators.TranslatePost(post, _client.CurrentUser.UserId, images);
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
                var api = string.Format("post?size={0}&page={1}&challenge={2}", size, page, challengeId);
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
                var postsDTo = PostDataTranslators.TranslatePosts(posts);
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
                var success = await _client.InvokeApiAsync<bool>(api, HttpMethod.Put, param);
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
                var success = await _client.InvokeApiAsync<bool>(api, HttpMethod.Put, param);
                return success;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }


        public async Task<List<AzurePost>> SyncPostAsync(string challengeId, int page = 0, int size = 10)
        {
            //ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            //try
            //{
            //    await this.Client.SyncContext.PushAsync();

            //    // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
            //    // Use a different query name for each unique query in your program.
            //    await this.azurePost.PullAsync("allPostItems", this.azurePost.CreateQuery().Where(po => po.Challenge == challengeId));
            //    var posts = await this.azurePost.CreateQuery().Where(po => po.Challenge == challengeId).ToListAsync();
            //    return posts;
            //}
            //catch (MobileServicePushFailedException exc)
            //{
            //    ExceptionService.Handle(exc);
            //    if (exc.PushResult != null)
            //    {
            //        syncErrors = exc.PushResult.Errors;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ExceptionService.Handle(ex);
            //    return null;
            //}
            //// Simple error/conflict handling.
            //if (syncErrors != null)
            //{
            //    foreach (var error in syncErrors)
            //    {
            //        if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
            //        {
            //            // Update failed, revert to server's copy
            //            await error.CancelAndUpdateItemAsync(error.Result);
            //        }
            //        else
            //        {
            //            // Discard local change
            //            await error.CancelAndDiscardItemAsync();
            //        }

            //        // Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
            //    }
            //}
            return null;
        }
    }
}