using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Abstractions.Exceptions;
using Abstractions.Forms;
using Journey.Models.Post;
using Journey.Services.Buisness.Post.Dto;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.Post.Translators
{
    public static class PostDataTranslators
    {
        private const string VideoPlaceHolderPath = "http://bit.ly/2EiCAic";

        public static AzurePost TranslatePost(Models.Post.Post post, string account, List<string> images)
        {
            try
            {
                var postDto = new AzurePost();
                if (post != null)
                {
                    postDto.Account = account;
                    postDto.Status = post.Feed;
                    postDto.Activity = JsonConvert.SerializeObject(post.Activity);
                    postDto.Location = JsonConvert.SerializeObject(post.Location);
                    postDto.Measurements = JsonConvert.SerializeObject(post.Measuremnts);
                    postDto.Images = JsonConvert.SerializeObject(images);
                    postDto.Challenge = post.Challenge;
                }

                return postDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Post", ex);
            }
        }

        public static Models.Post.Post TranslatePost(AzurePost post)
        {
            var postDto = new Models.Post.Post();
            if (post != null)
            {
                postDto.Id = post.Id;
                postDto.Account = new Models.Account.Account
                {
                    Id = post.Account,
                    LastName = post.Lname,
                    FirstName = post.Fname,
                    Image = new Media {Path = post.Profile}
                };
                postDto.Liked = post.Liked.HasValue?post.Liked.Value:false;
                postDto.LikesCount = post.Likes;
                postDto.SharesCount = post.Shares;
                postDto.CommentsCount = post.Comments;
                postDto.Feed = post.Status;
                postDto.DateTime = post.CreatedAt.ToLocalTime();
                try
                {
                    postDto.Activity = JsonConvert.DeserializeObject<PostActivity>(post.Activity);
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    postDto.Location = JsonConvert.DeserializeObject<PostActivity>(post.Location);
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    postDto.Measuremnts = JsonConvert.DeserializeObject<List<ScaleMeasurment>>(post.Measurements);
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    var images = JsonConvert.DeserializeObject<List<string>>(post.Images);
                    if (images != null)
                    {
                        postDto.MediaList = new ObservableCollection<Media>();
                        foreach (string image in images)
                            postDto.MediaList.Add(new Media
                            {
                                Path = image,
                                Ext = Path.GetExtension(image),
                                Type = Path.GetExtension(image) == ".mp4" ? MediaType.Video : MediaType.Image,
                                Thumbnail = Path.GetExtension(image) == ".mp4" ? VideoPlaceHolderPath : image
                            });
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            return postDto;
        }

        public static List<Models.Post.Post> TranslatePosts(List<AzurePost> posts)
        {
            try
            {
                var postDtos = new List<Models.Post.Post>();
                foreach (AzurePost post in posts)
                {
                    Models.Post.Post postDto = TranslatePost(post);
                    postDtos.Add(postDto);
                }

                return postDtos;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Post", ex);
            }
        }
    }
}