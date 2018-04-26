using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models.Post;

namespace Journey.Services.Buisness.Post.Data
{
    public interface IPostDataService
    {
        Task<Models.Post.Post> AddPostAsync(Models.Post.Post post, List<string> images);
        Task<bool> DeletePostAsync(PostBase post);
        Task<List<Models.Post.Post>> GetPostsAsync(int page, int size, bool sync = false);
        Task<bool> LikeAsync(PostBase post);
        Task<bool> ShareAsync(PostBase post);
    }
}