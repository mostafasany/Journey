using Journey.Models.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tawasol.Services
{
    public interface IPostCommentService
    {
        Task<List<Comment>> GetCommentsAsync(string post, bool sync = false);

        Task<Comment> AddCommentAsync(string comment, string post);

        Task<bool> DeleteCommentAsync(string comment, string post);
    }
}
