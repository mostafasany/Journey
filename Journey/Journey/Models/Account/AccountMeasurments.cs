namespace Journey.Models.Account
{
    public class AccountMeasurments : Account
    {
        public AccountMeasurments(Account account)
        {
            SocialToken = account.SocialToken;
            SocialProvider = account.SocialProvider;
            Email = account.Email;
            Gender = account.Gender;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Id = account.Id;
            Image = account.Image;
        }

        //private List<ScaleMeasurment> measuremnts;
        //public List<ScaleMeasurment> Measuremnts
        //{
        //    get => measuremnts;
        //    set => SetProperty(ref measuremnts, value);
        //}
    }
}