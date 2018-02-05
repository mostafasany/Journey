using System.Collections.Generic;

namespace Tawasol.Models
{
    public class AccountMeasurments : Account
    {
        public AccountMeasurments(Account account)
        {
            this.SocialToken = account.SocialToken;
            this.SocialProvider = account.SocialProvider;
            this.Email = account.Email;
            this.Gender = account.Gender;
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
