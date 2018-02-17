using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace Journey.Models.Post
{
    public enum PostStatus
    {
        InProgress,
        HideProgress,
        Deleted,
        CommentsUpdated,
        Added
    }

    public class PostBase : BindableBase
    {
        private Account.Account account;

        private ObservableCollection<Comment> comments;

        private int commentsCount;

        private DateTime dateTime;

        private string feed;

        private string id;

        private bool liked;

        private int likesCount;

        private ObservableCollection<Media> mediaList;

        private PostType postType;

        private int sharesCount;
        public string Challenge { get; set; }

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

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

        public Account.Account Account
        {
            get => account;
            set => SetProperty(ref account, value);
        }

        public ObservableCollection<Media> MediaList
        {
            get => mediaList;
            set => SetProperty(ref mediaList, value);
        }

        public bool HasMediaList => MediaList != null && MediaList.Count > 0;

        public string Feed
        {
            get => feed;
            set => SetProperty(ref feed, value);
        }

        public PostType PostType
        {
            get => postType;
            set => SetProperty(ref postType, value);
        }

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

        public ObservableCollection<Comment> Comments
        {
            get => comments;
            set => SetProperty(ref comments, value);
        }

        public int LikesCount
        {
            get => likesCount;
            set => SetProperty(ref likesCount, value);
        }

        public int CommentsCount
        {
            get => commentsCount;
            set => SetProperty(ref commentsCount, value);
        }

        public int SharesCount
        {
            get => sharesCount;
            set => SetProperty(ref sharesCount, value);
        }
    }
}