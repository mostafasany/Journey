namespace Journey.Models.Account
{
    public enum FriendShipEnum
    {
        Rejected = 0,
        Requested = 1,
        Approved = 2,
        Nothing
    }

    public class FriendShip : Account
    {
        private string _friendShipStatus;

        public string FriendShipStatus
        {
            get => _friendShipStatus;
            set => SetProperty(ref _friendShipStatus, value);
        }

        public string FriendShipId { get; set; }

        public FriendShipEnum FriendShipEnum
        {
            get
            {
                if (string.IsNullOrEmpty(FriendShipStatus)) return FriendShipEnum.Nothing;

                return (FriendShipEnum) int.Parse(FriendShipStatus);
            }
        }
    }
}