using System;
using System.Collections.Generic;
using Abstractions.Exceptions;
using Abstractions.Forms;
using Journey.Models.Post;
using Journey.Services.Buisness.PostComment.Dto;

namespace Journey.Services.Buisness.PostComment.Translators
{
    public static class CommentsDataTranslators
    {
        public static AzurePostComments TranslateComment(string comment, string post, string account)
        {
            try
            {
                var commentDto = new AzurePostComments();
                if (comment != null)
                {
                    commentDto.Account = account;
                    commentDto.Comment = comment;
                    commentDto.Post = post;
                    commentDto.CreatedAt = DateTime.Now;
                }

                return commentDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Post", ex);
            }
        }

        public static Comment TranslateComment(AzurePostComments comment)
        {
            try
            {
                var commentDto = new Comment();
                if (comment != null)
                {
                    commentDto.Account = new Models.Account.Account
                    {
                        Id = comment.Account,
                        FirstName = comment.Fname,
                        LastName = comment.Lname,
                        Image = new Media {Path = comment.Profile}
                    };
                    commentDto.CommentText = comment.Comment;
                    commentDto.PostId = comment.Post;
                    commentDto.DateTime = comment.CreatedAt.ToLocalTime();
                    commentDto.Id = comment.Id;
                }

                return commentDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Post", ex);
            }
        }

        public static List<Comment> TranslateComments(List<AzurePostComments> posts)
        {
            try
            {
                var postDtos = new List<Comment>();
                foreach (AzurePostComments post in posts)
                {
                    Comment postDto = TranslateComment(post);
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