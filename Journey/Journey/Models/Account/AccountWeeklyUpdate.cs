namespace Tawasol.Models
{
    public class AccountWeeklyUpdate : Account
    {
        public AccountWeeklyUpdate(Account account)
        {
            FirstName = account.FirstName;
            LastName = account.LastName;
            Id = account.Id;
            Image = account.Image;
        }

        private double distanceCovered;
        public double DistanceCovered
        {
            get => distanceCovered;
            set => SetProperty(ref distanceCovered, value);
        }

        public string DistanceCoveredWithUnit => $"{DistanceCovered} Km";

        private int numberOfWorkouts;
        public int NumberOfWorkouts
        {
            get => numberOfWorkouts;
            set => SetProperty(ref numberOfWorkouts, value);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
