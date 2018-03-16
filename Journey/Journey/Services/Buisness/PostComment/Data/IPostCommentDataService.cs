using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.PostComment.Data
{
    public interface IPostCommentDataService
    {
        Task<Comment> AddCommentAsync(string comment, string post);
        Task<bool> DeleteCommentAsync(string comment, string post);
        Task<List<Comment>> GetCommentsAsync(string post, int page, int size, bool sync = false);
    }
}