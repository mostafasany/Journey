using Prism.Mvvm;

namespace Journey.Models
{
    public enum NotificationType { Notification, FriendRequest, ChallengeRequest }

    public class Notifications : BindableBase
    {
        private Account.Account account;

        private string deepLink;
        private string id;

        private string message;

        private string title;
        public NotificationType NotificationType;

        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public string DeepLink
        {
            get => deepLink;
            set => SetProperty(ref deepLink, value);
        }

        public Account.Account Account
        {
            get => account;
            set => SetProperty(ref account, value);
        }
    }
}