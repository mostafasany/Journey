using System;
using Android.Gms.Common.Apis;
using Android.OS;

namespace Journey.Droid.Services.Fitness
{
    class ClientConnectionCallback : Java.Lang.Object, GoogleApiClient.IConnectionCallbacks
    {
        public const string TAG = "BasicSensorsApi";
        public Action OnConnectedImpl { get; set; }

        public void OnConnected(Bundle connectionHint)
        {
            Android.Util.Log.Info(TAG, "Connected!!!");

            OnConnectedImpl();
        }

        public void OnConnectionSuspended(int cause)
        {
            if (cause == GoogleApiClient.ConnectionCallbacksConsts.CauseNetworkLost)
            {
                Android.Util.Log.Info(TAG, "Connection lost.  Cause: Network Lost.");
            }
            else if (cause == GoogleApiClient.ConnectionCallbacksConsts.CauseServiceDisconnected)
            {
                Android.Util.Log.Info(TAG, "Connection lost.  Reason: Service Disconnected");
            }
        }
    }

}
