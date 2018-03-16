namespace Journey.Models.Account
{
    public class AccountWeeklyUpdate : Account
    {
        private double distanceCovered;

        private int numberOfWorkouts;

        public AccountWeeklyUpdate(Account account)
        {
            FirstName = account.FirstName;
            LastName = account.LastName;
            Id = account.Id;
            Image = account.Image;
        }

        public double DistanceCovered
        {
            get => distanceCovered;
            set => SetProperty(ref distanceCovered, value);
        }

        public string DistanceCoveredWithUnit => $"{DistanceCovered} Km";

        public int NumberOfWorkouts
        {
            get => numberOfWorkouts;
            set => SetProperty(ref numberOfWorkouts, value);
        }

        public override string ToString() => Name;
    }
}