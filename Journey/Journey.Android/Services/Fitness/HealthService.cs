using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Android.App;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Result;

[assembly: Xamarin.Forms.Dependency(typeof(Journey.Droid.HealthService))]
namespace Journey.Droid
{
    public class HealthService : IHealthService
    {
        MainActivity _mainActivity;
        public HealthService()
        {
            _mainActivity = Xamarin.Forms.Forms.Context as MainActivity;
        }

        Android.Gms.Common.Apis.GoogleApiClient mClient;
        public const string TAG = "BasicSensorsApi";
        public bool AuthInProgress { get; set; }
        const int REQUEST_OAUTH = 1;

        const string AUTH_PENDING = "auth_state_pending";

        public event HealthDataEventHandler HealthDataChanged;

        void BuildFitnessClient()
        {
            var clientConnectionCallback = new Services.Fitness.ClientConnectionCallback();
            clientConnectionCallback.OnConnectedImpl = ()
                =>
            {
                GetRunningWalkingDistanceAsync();
                GetCaloriesAsync();
            };
            if (mClient == null)
            {
                mClient = new Android.Gms.Common.Apis.GoogleApiClient.Builder(_mainActivity)
                    .AddApi(FitnessClass.SENSORS_API)
                    .AddApi(FitnessClass.HISTORY_API)
                    .AddScope(new Android.Gms.Common.Apis.Scope(Android.Gms.Common.Scopes.FitnessActivityReadWrite))
                    .AddScope(new Android.Gms.Common.Apis.Scope(Android.Gms.Common.Scopes.FitnessBodyReadWrite))
                    .AddScope(new Android.Gms.Common.Apis.Scope(Android.Gms.Common.Scopes.FitnessLocationRead))
                    .AddConnectionCallbacks(clientConnectionCallback)
                    .AddOnConnectionFailedListener(FailedToConnect)
                    .Build();
                MainActivity.mClient = mClient;
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
            if (!AuthInProgress)
            {
                try
                {
                    Android.Util.Log.Info(TAG, "Attempting to resolve failed connection");
                    AuthInProgress = true;
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
                AuthInProgress = false;
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
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(mClient, DataType.TypeCaloriesExpended);
            ShowDataSet(result.Total, Unit.KCAL.ToString());
        }

        public async Task GetHeightAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(mClient, DataType.TypeHeight);
            ShowDataSet(result.Total, Unit.CM.ToString());
        }

        public async Task GetRunningWalkingDistanceAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(mClient, DataType.TypeDistanceDelta);
            ShowDataSet(result.Total, Unit.RunningWalking.ToString());
        }

        public async Task GetStepsAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(mClient, DataType.TypeStepCountDelta);
            ShowDataSet(result.Total, Unit.Steps.ToString());
        }

        public async Task GetWeightAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(mClient, DataType.TypeWeight);
            ShowDataSet(result.Total, Unit.KG.ToString());
        }

        private void ShowDataSet(DataSet dataSet, string unit)
        {
            foreach (var item in dataSet.DataPoints)
            {
                foreach (var field in item.DataType.Fields)
                {
                    Value val = item.GetValue(field);
                    RaiseDataChanged(unit, val.ToString());
                }
            }
        }

        void RaiseDataChanged(string unit, string measure)
        {
            HealthDataChanged?.Invoke(this, new HealthDataEventArgs
            {
                Data = new System.Collections.Generic.Dictionary<string, string>
                {
                    {"Unit",unit},
                    {"Measure",measure}
                }
            });
        }
    }
}
