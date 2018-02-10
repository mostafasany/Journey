using Journey.Models.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tawasol.Services.Data
{
    public interface IPostCommentDataService
    {
        Task<Comment> AddCommentAsync(string comment, string post);
        Task<List<Comment>> GetCommentsAsync(string post, int page, int size, bool sync = false);
        Task<bool> DeleteCommentAsync(string comment, string post);
    }
}
