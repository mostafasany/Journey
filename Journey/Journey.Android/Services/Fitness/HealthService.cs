using System.Threading.Tasks;
using Abstractions.Services.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(Journey.Droid.HealthService))]
namespace Journey.Droid
{
    public class HealthService : IHealthService
    {
        Android.App.Activity _mainActivity;
        public HealthService()
        {
            _mainActivity = Xamarin.Forms.Forms.Context as Android.App.Activity;
        }
        Android.Gms.Common.Apis.GoogleApiClient mClient;
        public const string TAG = "BasicSensorsApi";

        const int REQUEST_OAUTH = 1;

        const string AUTH_PENDING = "auth_state_pending";
        bool authInProgress;


        public event HealthDataEventHandler HealthDataChanged;

        void BuildFitnessClient()
        {
            var clientConnectionCallback = new Services.Fitness.ClientConnectionCallback();
            clientConnectionCallback.OnConnectedImpl = () => Services.Fitness.FitnessService.FindFitnessDataSources(mClient);
            if (mClient == null)
            {
                mClient = new Android.Gms.Common.Apis.GoogleApiClient.Builder(_mainActivity)
                    .AddApi(Android.Gms.Fitness.FitnessClass.SENSORS_API)
                    .AddScope(new Android.Gms.Common.Apis.Scope(Android.Gms.Common.Scopes.FitnessLocationRead))
                    .AddConnectionCallbacks(clientConnectionCallback)
                   .AddOnConnectionFailedListener(FailedToConnect)
                    .Build();
            }
            if (!mClient.IsConnecting && !mClient.IsConnected)
            {
                mClient.Connect();
            }
        }

        void FailedToConnect(Android.Gms.Common.ConnectionResult result)
        {
            Android.Util.Log.Info(TAG, "Connection failed. Cause: " + result);
            if (!result.HasResolution)
            {
                // Show the localized error dialog
                Android.Gms.Common.GooglePlayServicesUtil.GetErrorDialog(result.ErrorCode, _mainActivity, 0).Show();
                return;
            }
            if (!authInProgress)
            {
                try
                {
                    Android.Util.Log.Info(TAG, "Attempting to resolve failed connection");
                    authInProgress = true;
                    result.StartResolutionForResult(_mainActivity, REQUEST_OAUTH);
                }
                catch (Android.Content.IntentSender.SendIntentException e)
                {
                    Android.Util.Log.Error(TAG, "Exception while starting resolution activity", e);
                }
            }
        }

        public async Task<bool> Authenticate()
        {
            try
            {
                //authInProgress = false;
                BuildFitnessClient();
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }

        }

        public async Task GetAgeAsync()
        {

        }

        public async Task GetCaloriesAsync()
        {

        }

        public async Task GetHeightAsync()
        {

        }

        public async Task GetRunningWalkingDistanceAsync()
        {
        }

        public async Task GetStepsAsync()
        {

        }

        public async Task GetWeightAsync()
        {

        }
    }
}
