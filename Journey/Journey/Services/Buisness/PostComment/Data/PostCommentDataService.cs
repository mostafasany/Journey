﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Post;
using Journey.Services.Azure;
using Journey.Services.Buisness.Post.Dto;
using Journey.Services.Buisness.Post.Translators;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace Journey.Services.Buisness.PostComment.Data
{
    public class PostCommentDataService :  IPostCommentDataService
    {
        private readonly IMobileServiceSyncTable<AzurePostComments> azureComment;
        private readonly MobileServiceClient _client;
        public PostCommentDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            this.azureComment = _client.GetSyncTable<AzurePostComments>();
        }

        public async Task<Comment> AddCommentAsync(string comment, string post)
        {
            try
            {
                if (comment == null)
                    return null;
                string account = _client.CurrentUser.UserId;
                AzurePostComments commentDto = PostDataTranslators.TranslateComment(comment, post, account);

                await azureComment.InsertAsync(commentDto);

                await SyncCommentAsync(post);

                var comm = PostDataTranslators.TranslateComment(commentDto);
                return comm;
            }
            catch (System.Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }


        public async Task<List<Comment>> GetCommentsAsync(string post, int page, int size, bool sync = false)
        {
            try
            {
                List<AzurePostComments> comments = null;
               //// comments = await Client.InvokeApiAsync<List<AzurePostComments>>(api, HttpMethod.Get, null);
                if (sync)
                {
                    comments = await SyncCommentAsync(post);
                }
                if (comments == null || comments.Count == 0)
                {
                    comments = await this.azureComment.Where(po => po.Post == post).ToListAsync();
                }
                if (comments == null || comments.Count == 0)
                {
                    comments = await SyncCommentAsync(post);
                }
                if (comments == null || comments.Count == 0)
                {
                    return null;
                }

                List<Comment> commentsDTo = PostDataTranslators.TranslateComments(comments);
                return commentsDTo;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<bool> DeleteCommentAsync(string comment, string post)
        {
            try
            {
                if (string.IsNullOrEmpty(comment))
                    return false;

                await azureComment.DeleteAsync(new AzurePostComments() { Id = comment });

                await SyncCommentAsync(post);

                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<List<AzurePostComments>> SyncCommentAsync(string post)
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this._client.SyncContext.PushAsync();

                // The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                // Use a different query name for each unique query in your program.
                await this.azureComment.PullAsync(Guid.NewGuid().ToString(), this.azureComment.CreateQuery().Where(po => po.Post == post));

                List<AzurePostComments> comments = await this.azureComment.Where(po => po.Post == post).ToListAsync();
                return comments;
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }


            // Simple error/conflict handling.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        // Update failed, revert to server's copy
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change
                        await error.CancelAndDiscardItemAsync();
                    }

                    // Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
            return null;
        }

    }
}
