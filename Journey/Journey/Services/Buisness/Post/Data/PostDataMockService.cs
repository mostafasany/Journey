using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Journey.Models.Post;
using Journey.Services.Buisness.Post.Dto;
using Journey.Services.Buisness.Post.Translators;

namespace Journey.Services.Buisness.Post.Data
{
    public class PostDataMockService : IPostDataService
    {
        private readonly ISerializerService _serializerService;

        public PostDataMockService(ISerializerService serializerService)
        {
            _serializerService = serializerService;
        }

        public async Task<Models.Post.Post> AddPostAsync(Models.Post.Post post, List<string> images)
        {
            try
            {
                if (post == null)
                    return null;
                var assembly = typeof(PostDataMockService).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream("Journey.Services.Mocks.PostMock.xml");
                string text;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                var postDto = _serializerService.DeserializeFromString<AzurePost>(text);

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
                var assembly = typeof(PostDataMockService).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream("Journey.Services.Mocks.PostsMock.xml");
                string text;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
                var posts = _serializerService.DeserializeFromString<List<AzurePost>>(text);

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
                return true;
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
                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }
    }
}