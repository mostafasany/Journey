using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.Post.Data
{
    public interface IPostDataService
    {
        Task<Models.Post.Post> AddPostAsync(Models.Post.Post post, List<string> images);
        Task<List<Models.Post.Post>> GetPostsAsync(string challengeId, int page, int size, bool sync = false);
        Task<bool> DeletePostAsync(PostBase post);

        Task<bool> LikeAsync(PostBase post);
        Task<bool> ShareAsync(PostBase post);
    }
}