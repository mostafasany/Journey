using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Result;
using Android.Util;
using Journey.Droid;
using Journey.Droid.Services.Fitness;
using Xamarin.Forms;
using DataType = Android.Gms.Fitness.Data.DataType;

[assembly: Dependency(typeof(HealthService))]

namespace Journey.Droid
{
    public class HealthService : IHealthService
    {
        private readonly MainActivity _mainActivity;

        private GoogleApiClient _mClient;
        public const string Tag = "BasicSensorsApi";
        private const int RequestOauth = 1;
        public HealthService() => _mainActivity = Forms.Context as MainActivity;
        public bool AuthInProgress { get; set; }

        public event HealthDataEventHandler HealthDataChanged;

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                AuthInProgress = false;
                BuildFitnessClient();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task GetAgeAsync() => null;

        public async Task GetCaloriesAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(_mClient, DataType.TypeCaloriesExpended);
            ShowDataSet(result.Total, Unit.KCAL.ToString());
        }

        public async Task GetHeightAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(_mClient, DataType.TypeHeight);
            ShowDataSet(result.Total, Unit.CM.ToString());
        }

        public async Task GetRunningWalkingDistanceAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(_mClient, DataType.TypeDistanceDelta);
            ShowDataSet(result.Total, Unit.RunningWalking.ToString());
        }

        public async Task GetStepsAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(_mClient, DataType.TypeStepCountDelta);
            ShowDataSet(result.Total, Unit.Steps.ToString());
        }

        public async Task GetWeightAsync()
        {
            DailyTotalResult result = await FitnessClass.HistoryApi.ReadDailyTotalAsync(_mClient, DataType.TypeWeight);
            ShowDataSet(result.Total, Unit.KG.ToString());
        }

        private void BuildFitnessClient()
        {
            var clientConnectionCallback = new ClientConnectionCallback();
            clientConnectionCallback.OnConnectedImpl = async ()
                =>
            {
                await GetRunningWalkingDistanceAsync();
                await GetCaloriesAsync();
            };
            if (_mClient == null)
            {
                _mClient = new GoogleApiClient.Builder(_mainActivity)
                    .AddApi(FitnessClass.SENSORS_API)
                    .AddApi(FitnessClass.HISTORY_API)
                    .AddScope(new Scope(Scopes.FitnessActivityReadWrite))
                    .AddScope(new Scope(Scopes.FitnessBodyReadWrite))
                    .AddScope(new Scope(Scopes.FitnessLocationRead))
                    //.AddConnectionCallbacks(clientConnectionCallback)
                    .AddOnConnectionFailedListener(FailedToConnect)
                    .Build();
                MainActivity.MClient = _mClient;
            }

            if (!_mClient.IsConnecting && !_mClient.IsConnected) _mClient.Connect();
        }

        private void FailedToConnect(ConnectionResult result)
        {
            Log.Info(Tag, "Connection failed. Cause: " + result);
            if (!result.HasResolution)
            {
                // Show the localized error dialog
                GooglePlayServicesUtil.GetErrorDialog(result.ErrorCode, _mainActivity, 0).Show();
                return;
            }

            if (!AuthInProgress)
                try
                {
                    Log.Info(Tag, "Attempting to resolve failed connection");
                    AuthInProgress = true;
                    result.StartResolutionForResult(_mainActivity, RequestOauth);
                }
                catch (IntentSender.SendIntentException e)
                {
                    Log.Error(Tag, "Exception while starting resolution activity", e);
                }
        }

        private void RaiseDataChanged(string unit, string measure)
        {
            HealthDataChanged?.Invoke(this, new HealthDataEventArgs
            {
                Data = new Dictionary<string, string>
                {
                    {"Unit", unit},
                    {"Measure", measure}
                }
            });
        }

        private void ShowDataSet(DataSet dataSet, string unit)
        {
            foreach (DataPoint item in dataSet.DataPoints)
            foreach (Field field in item.DataType.Fields)
            {
                Value val = item.GetValue(field);
                RaiseDataChanged(unit, val.ToString());
            }
        }
    }
}