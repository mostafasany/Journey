using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Post;
using Journey.Services.Buisness.Post;
using Journey.Services.Buisness.PostComment.Data;

namespace Journey.Services.Buisness.PostComment
{
    public class PostCommentService : IPostCommentService
    {
        private const int PostPageSize = 10;
        private readonly IPostCommentDataService postDataService;
        private readonly IPostService postService;
        public PostCommentService(IPostCommentDataService _postDataService, IPostService _postService)
        {
            postDataService = _postDataService;
            postService = _postService;
        }

        public async Task<Comment> AddCommentAsync(string comment, string post)
        {
            try
            {
                postService.PostStatusChanged(null, PostStatus.InProgress);
                var commentDTo = await postDataService.AddCommentAsync(comment, post);
                if (commentDTo != null)
                    postService.PostStatusChanged(new PostBase() { Id = post }, PostStatus.CommentsUpdated);
                return commentDTo;
            }

            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message, ex);
            }
        }

        public async Task<List<Comment>> GetCommentsAsync(string post, bool sync = false)
        {
            try
            {
                List<Comment> commentList = await postDataService.GetCommentsAsync(post, 0, PostPageSize, sync);
                if (commentList == null)
                    return null;
                return commentList;
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message, ex);
            }
        }
        public async Task<bool> DeleteCommentAsync(string comment, string post)
        {
            try
            {
                var deleted = await postDataService.DeleteCommentAsync(comment, post);
                return deleted;
            }
            catch (Exception ex)
            {
                throw new BuisnessException(ex.Message, ex);
            }
        }
    }
}
