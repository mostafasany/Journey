﻿using System;
using Prism.Mvvm;

namespace Journey.Models.Post
{
    public class Comment : BindableBase
    {
        private bool _mine;
        private Account.Account account;

        private string commentText;

        private DateTime dateTime;
        private string id;

        private string postId;

        public string Id
        {
            get => id;
            set
            {
                id = value;
                RaisePropertyChanged();
            }
        }

        public bool Mine
        {
            get => _mine;
            set => SetProperty(ref _mine, value);
        }

        public string PostId
        {
            get => postId;
            set
            {
                postId = value;
                RaisePropertyChanged();
            }
        }

        public DateTime DateTime
        {
            get => dateTime;
            set
            {
                dateTime = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(FormatedDate));
            }
        }

        public string FormatedDate => DateHelper.Format(dateTime);

        public Account.Account Account
        {
            get => account;
            set
            {
                account = value;
                RaisePropertyChanged();
            }
        }

        public string CommentText
        {
            get => commentText;
            set
            {
                commentText = value;
                RaisePropertyChanged();
            }
        }
    }
}