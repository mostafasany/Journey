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
        private readonly IPostCommentDataService _postDataService;
        private readonly IPostService _postService;

        public PostCommentService(IPostCommentDataService postDataService, IPostService postService)
        {
            _postDataService = postDataService;
            _postService = postService;
        }

        public async Task<Comment> AddCommentAsync(string comment, string post)
        {
            try
            {
                _postService.PostStatusChanged(null, PostStatus.InProgress);
                var commentDTo = await _postDataService.AddCommentAsync(comment, post);
                if (commentDTo != null)
                    _postService.PostStatusChanged(new PostBase {Id = post}, PostStatus.CommentsUpdated);
                return commentDTo;
            }

            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Comment>> GetCommentsAsync(string post, bool sync = false)
        {
            try
            {
                var commentList = await _postDataService.GetCommentsAsync(post, 0, PostPageSize, sync);
                return commentList;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteCommentAsync(string comment, string post)
        {
            try
            {
                var deleted = await _postDataService.DeleteCommentAsync(comment, post);
                return deleted;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}