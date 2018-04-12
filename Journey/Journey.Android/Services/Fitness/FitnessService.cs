﻿using System.Threading.Tasks;
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
            var dataSourcesResult = await FitnessClass.SensorsApi.FindDataSourcesAsync(mClient, new DataSourcesRequest.Builder()
                                                                                        .SetDataTypes(Android.Gms.Fitness.Data.DataType.TypeLocationSample)
                .SetDataSourceTypes(DataSource.TypeRaw)
                .Build());

            Log.Info(TAG, "Result: " + dataSourcesResult.Status);
            foreach (DataSource dataSource in dataSourcesResult.DataSources)
            {
                Log.Info(TAG, "Data source found: " + dataSource);
                Log.Info(TAG, "Data Source type: " + dataSource.DataType.Name);

                //Let's register a listener to receive Activity data!
                if (dataSource.DataType == Android.Gms.Fitness.Data.DataType.TypeLocationSample && mListener == null)
                {
                    Log.Info(TAG, "Data source for LOCATION_SAMPLE found!  Registering.");
                    await RegisterFitnessDataListener(mClient, dataSource, Android.Gms.Fitness.Data.DataType.TypeLocationSample);
                }
            }
        }

        static public async Task RegisterFitnessDataListener(GoogleApiClient mClient, DataSource dataSource, Android.Gms.Fitness.Data.DataType dataType)
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
