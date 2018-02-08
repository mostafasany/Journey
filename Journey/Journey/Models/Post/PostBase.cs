using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Journey.Models.Post
{
    public enum PostStatus
    {
        InProgress,
        Deleted,
        CommentsUpdated,
        Added
    }

    public class PostBase : BindableBase
    {
        public string Challenge { get; set; }

        private string id;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        private DateTime dateTime;
        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                SetProperty(ref dateTime, value);
                RaisePropertyChanged("FormatedDate");
            }
        }

        public string FormatedDate => DateHelper.Format(dateTime);


        private Account.Account account;
        public Account.Account Account
        {
            get => account;
            set => SetProperty(ref account, value);
        }

        private ObservableCollection<Media> mediaList;
        public ObservableCollection<Media> MediaList
        {
            get => mediaList;
            set => SetProperty(ref mediaList, value);
        }

        public bool HasMediaList
        {
            get => MediaList != null && MediaList.Count > 0;
        }

        private string feed;
        public string Feed
        {
            get => feed;
            set => SetProperty(ref feed, value);
        }

        private PostType postType;
        public PostType PostType
        {
            get => postType;
            set => SetProperty(ref postType, value);
        }

        private bool liked;
        public bool Liked
        {
            get => liked;
            set
            {
                SetProperty(ref liked, value);
                RaisePropertyChanged(nameof(NotLiked));
            }
        }


        public bool NotLiked => !Liked;

        private ObservableCollection<Comment> comments;
        public ObservableCollection<Comment> Comments
        {
            get => comments;
            set => SetProperty(ref comments, value);
        }

        private int likesCount;
        public int LikesCount
        {
            get => likesCount;
            set => SetProperty(ref likesCount, value);
        }

        private int commentsCount;
        public int CommentsCount
        {
            get => commentsCount;
            set => SetProperty(ref commentsCount, value);
        }

        private int sharesCount;
        public int SharesCount
        {
            get => sharesCount;
            set => SetProperty(ref sharesCount, value);
        }
    }
}

