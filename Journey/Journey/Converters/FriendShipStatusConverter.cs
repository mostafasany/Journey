using System;
using System.Globalization;
using Journey.Resources;
using Xamarin.Forms;

namespace Journey.Converters
{
    public class FriendShipStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string friendShipStatus = value?.ToString();
            if (friendShipStatus == "1")
            {
                return AppResource.FriendShip_Requested;
            }
            else if (friendShipStatus == "2")
            {
                return AppResource.FriendShip_Accepted;
            }
            else if (friendShipStatus == "0")
            {
                return AppResource.FriendShip_Rejected;
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}