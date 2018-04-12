using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Request;

namespace Journey.Droid.Services.Fitness
{
    class OnDataPointListener : Java.Lang.Object, IOnDataPointListener
    {
        public const string TAG = "BasicSensorsApi";
        public void OnDataPoint(DataPoint dataPoint)
        {
            foreach (var field in dataPoint.DataType.Fields)
            {
                Value val = dataPoint.GetValue(field);
                Android.Util.Log.Info(TAG, "Detected DataPoint field: " + field.Name);
                Android.Util.Log.Info(TAG, "Detected DataPoint value: " + val);
            }
        }

    }
}
