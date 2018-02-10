using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.PostComment
{
    public interface IPostCommentService
    {
        Task<List<Comment>> GetCommentsAsync(string post, bool sync = false);

        Task<Comment> AddCommentAsync(string comment, string post);

        Task<bool> DeleteCommentAsync(string comment, string post);
    }
}