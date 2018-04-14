using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Request;
using Android.Util;
using Java.Util.Concurrent;

namespace Journey.Droid.Services.Fitness
{
    static public class FitnessService
    {
        public const string TAG = "BasicSensorsApi";

        static public IOnDataPointListener mListener;

        static public async Task FindFitnessDataSources(GoogleApiClient mClient)
        {
            var dataSourceRequest = new DataSourcesRequest.Builder()
                                                        .SetDataTypes(
                                                              Android.Gms.Fitness.Data.DataType.TypeStepCountCumulative,
                                                              Android.Gms.Fitness.Data.DataType.TypeDistanceCumulative,
                                                              Android.Gms.Fitness.Data.DataType.TypeWeight,
                                                              Android.Gms.Fitness.Data.DataType.TypeHeight,
                                                              Android.Gms.Fitness.Data.DataType.TypeCaloriesConsumed,
                                                              Android.Gms.Fitness.Data.DataType.TypeCaloriesExpended,
                                                              Android.Gms.Fitness.Data.DataType.TypeLocationTrack,
                                                              Android.Gms.Fitness.Data.DataType.AggregateLocationBoundingBox)
                                                         .SetDataSourceTypes(DataSource.TypeRaw)
                                                         .Build();
            var dataSourcesResult =
                await FitnessClass.SensorsApi.FindDataSourcesAsync
                                                      (mClient, dataSourceRequest);

            Log.Info(TAG, "Result: " + dataSourcesResult.Status);
            var _mainActivity = Xamarin.Forms.Forms.Context as MainActivity;
            foreach (DataSource dataSource in dataSourcesResult.DataSources)
            {

                if (_mainActivity != null)
                    Android.Widget.Toast.MakeText(_mainActivity, "Found Data Sources", Android.Widget.ToastLength.Long).Show();

                Log.Info(TAG, "Data source found: " + dataSource);
                Log.Info(TAG, "Data Source type: " + dataSource.DataType.Name);

                //Let's register a listener to receive Activity data!
                if (dataSource.DataType == Android.Gms.Fitness.Data.DataType.TypeDistanceDelta && mListener == null)
                {
                    Log.Info(TAG, "Data source for LOCATION_SAMPLE found!  Registering.");
                    await RegisterFitnessDataListener(mClient, dataSource, Android.Gms.Fitness.Data.DataType.TypeLocationSample);
                }
            }
            if (_mainActivity != null)
                Android.Widget.Toast.MakeText(_mainActivity, "No Data Sources", Android.Widget.ToastLength.Long).Show();

        }

        static private async Task RegisterFitnessDataListener(GoogleApiClient mClient, DataSource dataSource, Android.Gms.Fitness.Data.DataType dataType)
        {
            // [START register_data_listener]
            mListener = new OnDataPointListener();
            var status = await FitnessClass.SensorsApi.AddAsync(mClient, new SensorRequest.Builder()
                .SetDataSource(dataSource) // Optional but recommended for custom data sets.
                .SetDataType(dataType) // Can't be omitted.
                .SetSamplingRate(10, TimeUnit.Seconds)
                .Build(),
                mListener);
            if (status.IsSuccess)
            {
                Log.Info(TAG, "Listener registered!");
            }
            else
            {
                Log.Info(TAG, "Listener not registered.");
            }
        }


        static public async Task UnregisterFitnessDataListener(GoogleApiClient mClient)
        {
            if (mListener == null)
            {
                return;
            }

            var status = await FitnessClass.SensorsApi.RemoveAsync(mClient, mListener);

            if (status.IsSuccess)
            {
                Log.Info(TAG, "Listener was removed!");
            }
            else
            {
                Log.Info(TAG, "Listener was not removed.");
            }

        }
    }
}
