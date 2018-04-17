using System;
using System.Threading.Tasks;
using Foundation;
using HealthKit;
using Abstractions.Services.Contracts;

[assembly: Xamarin.Forms.Dependency(typeof(Journey.iOS.Services.HealthService))]
namespace Journey.iOS.Services
{
    public class HealthService : IHealthService
    {
        public static HKHealthStore HealthStore { get; private set; }

        public event HealthDataEventHandler HealthDataChanged;

        NSSet DataTypesToWrite
        {
            get
            {
                return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.DietaryEnergyConsumed),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.ActiveEnergyBurned),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.Height),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.StepCount),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.DistanceWalkingRunning),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.BodyMass)
                });
            }
        }

        NSSet DataTypesToRead
        {
            get
            {
                return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.DietaryEnergyConsumed),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.ActiveEnergyBurned),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.Height),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.BodyMass),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.StepCount),
                    HKQuantityType.GetQuantityType (HKQuantityTypeIdentifierKey.DistanceWalkingRunning),
                    HKCharacteristicType.GetCharacteristicType (HKCharacteristicTypeIdentifierKey.DateOfBirth)
                });
            }
        }

        public bool AuthInProgress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public HealthService()
        {
            HealthStore = new HKHealthStore();
        }

        public async Task<bool> Authenticate()
        {
            try
            {
                if (HKHealthStore.IsHealthDataAvailable)
                {

                    var success = await HealthStore.RequestAuthorizationToShareAsync(DataTypesToWrite, DataTypesToRead);

                    if (!success.Item1)
                    {
                        Console.WriteLine("You didn't allow HealthKit to access these read/write data types. " +
                        "In your app, try to handle this error gracefully when a user decides not to provide access. " +
                        "If you're using a simulator, try it on a device.");
                        return false;
                    }
                    else
                    {
                        //GetRunningWalkingDistanceAsync();
                        //GetCaloriesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task GetAgeAsync()
        {
            try
            {
                if (await Authenticate())
                {
                    UpdateUsersAge();
                }
            }
            catch (Exception ex)
            {

            }
        }

        void UpdateUsersAge()
        {
            NSError error;
            int years = -1;
            NSDate dateOfBirth = HealthStore.GetDateOfBirth(out error);

            if (error != null)
            {
                Console.WriteLine("An error occured fetching the user's age information. " +
                "In your app, try to handle this gracefully. The error was: {0}", error);
                years = -1;
            }

            if (dateOfBirth == null)
                years = -1;

            var now = NSDate.Now;

            NSDateComponents ageComponents = NSCalendar.CurrentCalendar.Components(NSCalendarUnit.Year, dateOfBirth, now,
                                                 NSCalendarOptions.WrapCalendarComponents);

            nint usersAge = ageComponents.Year;
            years = int.Parse(usersAge.ToString());

            RaiseDataChanged(Unit.Year.ToString(), years.ToString());

        }

        public async Task GetCaloriesAsync()
        {
            FetchMostRecentDataForCalories((totalJoulesConsumed, error) =>
            {
                RaiseDataChanged(Unit.KCAL.ToString(), totalJoulesConsumed.ToString());
            });
        }

        public async Task GetHeightAsync()
        {
            try
            {
                if (await Authenticate())
                {
                    UpdateUsersHeight();
                }

            }
            catch (Exception ex)
            {
            }
        }

        void UpdateUsersHeight()
        {
            var heightType = HKQuantityType.GetQuantityType(HKQuantityTypeIdentifierKey.Height);
            FetchMostRecentData(heightType, (mostRecentQuantity, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("An error occured fetching the user's height information. " +
                    "In your app, try to handle this gracefully. The error was: {0}.", error.LocalizedDescription);
                    return;
                }

                double usersHeight = 0.0;

                if (mostRecentQuantity != null)
                {
                    var heightUnit = HKUnit.Meter;
                    usersHeight = mostRecentQuantity.GetDoubleValue(heightUnit);
                    usersHeight *= 100;
                }

                RaiseDataChanged(Unit.CM.ToString(), usersHeight.ToString());

            });
        }

        public async Task GetStepsAsync()
        {
            FetchMostRecentDataForSteps((totalJoulesConsumed, error) =>
            {
                RaiseDataChanged(Unit.Steps.ToString(), totalJoulesConsumed.ToString());
            });

        }

        void FetchMostRecentDataForSteps(Action<double, NSError> completionHandler)
        {
            var calendar = NSCalendar.CurrentCalendar;

            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddDays(1);

            var sampleType = HKQuantityType.GetQuantityType(HKQuantityTypeIdentifierKey.StepCount);
            var predicate = HKQuery.GetPredicateForSamples((NSDate)startDate, (NSDate)endDate, HKQueryOptions.StrictStartDate);

            var query = new HKStatisticsQuery(sampleType, predicate, HKStatisticsOptions.CumulativeSum,
                            (HKStatisticsQuery resultQuery, HKStatistics results, NSError error) =>
                            {

                                if (error != null && completionHandler != null)
                                    completionHandler(0.0f, error);

                                var totalCalories = results.SumQuantity();
                                if (totalCalories == null)
                                    totalCalories = HKQuantity.FromQuantity(HKUnit.Count, 0.0);

                                if (completionHandler != null)
                                    completionHandler(totalCalories.GetDoubleValue(HKUnit.Count), error);
                            });

            HealthStore.ExecuteQuery(query);
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

        public async Task GetWeightAsync()
        {
            try
            {
                if (await Authenticate())
                {
                    UpdateUsersWeight();
                }

            }
            catch (Exception ex)
            {
            }
        }

        void UpdateUsersWeight()
        {
            var weightType = HKQuantityType.GetQuantityType(HKQuantityTypeIdentifierKey.BodyMass);
            FetchMostRecentData(weightType, (mostRecentQuantity, error) =>
            {
                if (error != null)
                {
                    Console.WriteLine("An error occured fetching the user's age information. " +
                    "In your app, try to handle this gracefully. The error was: {0}", error.LocalizedDescription);
                    return;
                }

                double usersWeight = 0.0;

                if (mostRecentQuantity != null)
                {
                    var weightUnit = HKUnit.Gram;
                    usersWeight = mostRecentQuantity.GetDoubleValue(weightUnit);
                    usersWeight /= 1000;
                }

                RaiseDataChanged(Unit.KG.ToString(), usersWeight.ToString());


            }
             );

        }

        void FetchMostRecentData(HKQuantityType quantityType, Action<HKQuantity, NSError> completion)
        {
            var timeSortDescriptor = new NSSortDescriptor(HKSample.SortIdentifierEndDate, false);
            var query = new HKSampleQuery(quantityType, null, 1, new NSSortDescriptor[] { timeSortDescriptor },
                            (HKSampleQuery resultQuery, HKSample[] results, NSError error) =>
                            {
                                if (completion != null && error != null)
                                {
                                    completion(null, error);
                                    return;
                                }

                                HKQuantity quantity = null;
                                if (results.Length != 0)
                                {
                                    var quantitySample = (HKQuantitySample)results[results.Length - 1];
                                    quantity = quantitySample.Quantity;
                                }

                                if (completion != null)
                                    completion(quantity, error);
                            });

            HealthStore.ExecuteQuery(query);
        }

        void FetchMostRecentDataForCalories(Action<double, NSError> completionHandler)
        {
            var calendar = NSCalendar.CurrentCalendar;

            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddDays(1);

            var sampleType = HKQuantityType.GetQuantityType(HKQuantityTypeIdentifierKey.DietaryEnergyConsumed);
            var predicate = HKQuery.GetPredicateForSamples((NSDate)startDate, (NSDate)endDate, HKQueryOptions.StrictStartDate);

            var query = new HKStatisticsQuery(sampleType, predicate, HKStatisticsOptions.CumulativeSum,
                            (HKStatisticsQuery resultQuery, HKStatistics results, NSError error) =>
                            {

                                if (error != null && completionHandler != null)
                                    completionHandler(0.0f, error);

                                var totalCalories = results.SumQuantity();
                                if (totalCalories == null)
                                    totalCalories = HKQuantity.FromQuantity(HKUnit.Joule, 0.0);

                                if (completionHandler != null)
                                    completionHandler(totalCalories.GetDoubleValue(HKUnit.Joule), error);
                            });

            HealthStore.ExecuteQuery(query);
        }

        void FetchMostRecentDataForDistance(Action<double, NSError> completionHandler)
        {
            var calendar = NSCalendar.CurrentCalendar;

            var startDate = DateTime.Now.Date;
            var endDate = startDate.AddDays(1);

            var sampleType = HKQuantityType.GetQuantityType(HKQuantityTypeIdentifierKey.DistanceWalkingRunning);
            var predicate = HKQuery.GetPredicateForSamples((NSDate)startDate, (NSDate)endDate, HKQueryOptions.StrictStartDate);

            var query = new HKStatisticsQuery(sampleType, predicate, HKStatisticsOptions.CumulativeSum,
                            (HKStatisticsQuery resultQuery, HKStatistics results, NSError error) =>
                            {

                                if (error != null && completionHandler != null)
                                    completionHandler(0.0f, error);

                                var totalCalories = results.SumQuantity();
                                if (totalCalories == null)
                                    totalCalories = HKQuantity.FromQuantity(HKUnit.Meter, 0.0);

                                if (completionHandler != null)
                                    completionHandler(totalCalories.GetDoubleValue(HKUnit.Meter), error);
                            });

            HealthStore.ExecuteQuery(query);
        }

        public async Task GetRunningWalkingDistanceAsync()
        {
            FetchMostRecentDataForDistance((totalJoulesConsumed, error) =>
            {
                RaiseDataChanged(Unit.RunningWalking.ToString(), totalJoulesConsumed.ToString());
            });
        }
    }
}
