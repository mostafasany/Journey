namespace Journey.Models.Account
{
    public class AccountWeeklyUpdate : Account
    {
        private double _distanceCovered;

        private int _numberOfWorkouts;

        public AccountWeeklyUpdate(Account account)
        {
            FirstName = account.FirstName;
            LastName = account.LastName;
            Id = account.Id;
            Image = account.Image;
        }

        public double DistanceCovered
        {
            get => _distanceCovered;
            set => SetProperty(ref _distanceCovered, value);
        }

        public string DistanceCoveredWithUnit => $"{DistanceCovered} Km";

        public int NumberOfWorkouts
        {
            get => _numberOfWorkouts;
            set => SetProperty(ref _numberOfWorkouts, value);
        }

        public override string ToString() => Name;
    }
}