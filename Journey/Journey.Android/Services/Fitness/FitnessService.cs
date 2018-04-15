using System.Threading.Tasks;
using Abstractions.Services.Contracts;
using Android.Gms.Common.Apis;
using Android.Gms.Fitness;
using Android.Gms.Fitness.Data;
using Android.Gms.Fitness.Request;
using Android.Util;
using Java.Util.Concurrent;
using Android.Gms.Fitness.Data;
namespace Journey.Droid.Services.Fitness
{
    static public class FitnessService
    {
        public const string TAG = "BasicSensorsApi";

        static public IOnDataPointListener mListener;

        static public async Task FindFitnessDataSources(GoogleApiClient mClient)
        {
            var dataSourceRequest = new DataSourcesRequest.Builder()
                                                          .SetDataTypes(Android.Gms.Fitness.Data.DataType.TypeStepCountDelta,
                                                                        Android.Gms.Fitness.Data.DataType.TypeStepCountCumulative)

                                                          //.SetDataSourceTypes(DataSource.TypeRaw)
                                                          .SetDataSourceTypes(DataSource.TypeDerived)
                                                         .Build();
            var dataSourcesResult =
                await FitnessClass.SensorsApi.FindDataSourcesAsync
                                                      (mClient, dataSourceRequest);
            var _mainActivity = Xamarin.Forms.Forms.Context as MainActivity;

            Log.Info(TAG, "Result: " + dataSourcesResult.Status);

            var dataSources = dataSourcesResult.DataSources;
            foreach (DataSource dataSource in dataSources)
            {
                if (_mainActivity != null)
                    Android.Widget.Toast.MakeText(_mainActivity, "1", Android.Widget.ToastLength.Long).Show();

                Log.Info(TAG, "Data source found: " + dataSource);
                Log.Info(TAG, "Data Source type: " + dataSource.DataType.Name);

                //Let's register a listener to receive Activity data!
                var stepsCountType = Android.Gms.Fitness.Data.DataType.TypeStepCountDelta;
                var dataSourceType = dataSource.DataType;
                if (dataSourceType == stepsCountType && mListener == null)
                {
                    // Log.Info(TAG, "Data source for LOCATION_SAMPLE found!  Registering.");
                    await RegisterFitnessDataListener(mClient, dataSource, stepsCountType);
                }
                else
                {
                    await RegisterFitnessDataListener(mClient, dataSource, stepsCountType);
                }
            }
        }

        static private async Task RegisterFitnessDataListener(GoogleApiClient mClient, DataSource dataSource, Android.Gms.Fitness.Data.DataType dataType)
        {
            // [START register_data_listener]
            var request = new SensorRequest.Builder()
                .SetDataSource(dataSource) // Optional but recommended for custom data sets.
                .SetDataType(dataType) // Can't be omitted.
                .SetSamplingRate(10, TimeUnit.Seconds)
                                         .Build();
            mListener = new OnDataPointListener();
            var status = await FitnessClass.SensorsApi.AddAsync(mClient, request, mListener);
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
