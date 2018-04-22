using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Models;
using Abstractions.Services.Contracts;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.Challenge;
using Journey.Services.Buisness.ChallengeActivity.Data;

namespace Journey.Services.Buisness.ChallengeActivity
{
    public class ChallengeActivityService : IChallengeActivityService
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeActivityDataService _challengeActivityDataService;
        private readonly ISettingsService _settingsService;
        private readonly ILocationService _locationService;
        private readonly IChallengeService _challengeService;
        private const string LastExerciseDate = "LastExerciseDate";
        private const double MinDistanceForWorkout = 0.2;

        public ChallengeActivityService(IChallengeActivityDataService challengeActivityDataService,
                                        IAccountService accountService,
                                        ISettingsService settingsService,
                                        IChallengeService challengeService,
                                        ILocationService locationService)
        {
            _accountService = accountService;
            _challengeActivityDataService = challengeActivityDataService;
            _locationService = locationService;
            _settingsService = settingsService;
            _challengeService = challengeService;
        }

        public async Task<ChallengeActivityLog> AddActivityAsync(ChallengeActivityLog log)
        {
            try
            {
                ChallengeActivityLog logDto = await _challengeActivityDataService.AddActivityAsync(log);
                return logDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<ChallengeActivityLog> UpdateActivityAsync(ChallengeActivityLog log)
        {
            try
            {
                ChallengeActivityLog logDto = await _challengeActivityDataService.UpdateActivityAsync(log);
                return logDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteActivityAsync(ChallengeActivityLog log)
        {
            try
            {
                bool status = await _challengeActivityDataService.DeleteActivityAsync(log);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<ChallengeActivityLog>> GetChallengeActivitiesAsync(string challengeId)
        {
            try
            {
                List<ChallengeActivityLog> logDto = await _challengeActivityDataService.GetChallengeActivitiesAsync(challengeId);
                return logDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<ChallengeActivityLog>> GetAccountActivitiesAsync()
        {
            try
            {
                List<ChallengeActivityLog> logDto = await _challengeActivityDataService.GetAccountActivitiesAsync();
                return logDto;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<ObservableChallengeProgressGroupCollection<AccountChallengeProgress>>> GetChallengePorgessAsync(string challengeId)
        {
            try
            {
                List<ChallengeActivityLog> challengeProgress = await _challengeActivityDataService.GetChallengeActivitiesAsync(challengeId);
                if (!challengeProgress.Any())
                    return null;
                List<KeyGroupedChallengeProgress> orderedList = challengeProgress
                    .OrderBy(a => a.DatetTime)
                    .GroupBy(a => a.DatetTime.ToString("MMMM"))
                    .Select(g => new KeyGroupedChallengeProgress
                    {
                        Key = g.Key,
                        Accounts = g.OrderByDescending(a => a.Account.Name == _accountService.LoggedInAccount.Name).GroupBy(b => b.Account.Name).Select
                        (
                            b => new AccountChallengeProgress
                            {
                                Account = b.FirstOrDefault().Account,
                                TotalKm = b.Where(e => e is ChallengeKmActivityLog).Sum(s => ((ChallengeKmActivityLog)s).KM),
                                TotalExercises = b.Count(e => e is ChallengeWorkoutActivityLog),
                                TotalKcal = b.Where(e => e is ChallengeKcalActivityLog).Sum(s => ((ChallengeKcalActivityLog)s).Kcal),
                            }
                        ).ToList()
                    })
                    .ToList();

                var list = new List<ObservableChallengeProgressGroupCollection<AccountChallengeProgress>>();
                foreach (KeyGroupedChallengeProgress progress in orderedList)
                {
                    Models.Account.Account winnerAccountInExercises = null;
                    Models.Account.Account winnerAccountInKm = null;
                    IOrderedEnumerable<AccountChallengeProgress> orderedAccountExercises = progress.Accounts.OrderByDescending(a => a.TotalExercises);
                    IOrderedEnumerable<AccountChallengeProgress> orderedAccountKm = progress.Accounts.OrderByDescending(a => a.TotalKm);
                    var firstOrderedExercise = orderedAccountExercises.FirstOrDefault();
                    var lastOrderedExercise = orderedAccountExercises.LastOrDefault();
                    if (firstOrderedExercise.TotalExercises > lastOrderedExercise.TotalExercises)
                        winnerAccountInExercises = firstOrderedExercise.Account;

                    var firstOrderedKm = orderedAccountKm.FirstOrDefault();
                    if (firstOrderedKm.TotalKm > orderedAccountKm.LastOrDefault().TotalKm)
                        winnerAccountInKm = firstOrderedKm.Account;

                    var groupedData =
                        new ObservableChallengeProgressGroupCollection<AccountChallengeProgress>(progress.Key, progress.Accounts, winnerAccountInKm, winnerAccountInExercises);

                    list.Add(groupedData);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<ChallengeActivityLog> AddExerciseActivityAsync(Location myLocation)
        {
            try
            {
                Models.Challenge.Challenge _challenge = null;
                Location workoutLocation = null;
                string challengeId = null;
                if (!string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId))
                {
                    _challenge = await _challengeService.GetChallengeAsync(_accountService.LoggedInAccount.ChallengeId);
                    challengeId = _challenge.Id;
                    workoutLocation = _challenge.SelectedLocation;
                    double near = _locationService.DistanceBetweenPlaces(myLocation.Lng, myLocation.Lat, _challenge.SelectedLocation.Lng, _challenge.SelectedLocation.Lat);
                    if (near > MinDistanceForWorkout)
                    {
                        challengeId = "";
                    }
                }
                else
                {
                    workoutLocation = myLocation;
                }

                string date = await _settingsService.Get(LastExerciseDate);
                DateTime.TryParse(date, out DateTime parsedDate);
                if (parsedDate.Date != DateTime.Now.Date)
                {
                    await _settingsService.Set(LastExerciseDate, DateTime.Now.Date.ToString(CultureInfo.InvariantCulture));
                    var activity = new ChallengeWorkoutActivityLog
                    {
                        Account = _accountService.LoggedInAccount,
                        Challenge = challengeId,
                        DatetTime = DateTime.Now,
                        Location = workoutLocation
                    };
                    var newActivity = await AddActivityAsync(activity);
                    return newActivity;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}