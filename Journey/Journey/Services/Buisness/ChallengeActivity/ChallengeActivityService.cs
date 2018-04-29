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
        private readonly IChallengeService _challengeService;
        private readonly ILocationService _locationService;
        private const double MinDistanceForWorkout = 0.2;
        public ChallengeActivityService(IChallengeActivityDataService challengeActivityDataService,
            IAccountService accountService,
            IChallengeService challengeService,
            ILocationService locationService)
        {
            _accountService = accountService;
            _challengeActivityDataService = challengeActivityDataService;
            _locationService = locationService;
            _challengeService = challengeService;
        }

        public async Task<ChallengeActivityLog> AddUpdateActivityAsync(ChallengeActivityLog log)
        {
            try
            {
                ChallengeActivityLog logDto = await _challengeActivityDataService.AddUpdateActivityAsync(log);
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
                if (challengeProgress == null)
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
                                TotalKcal = b.Where(e => e is ChallengeKcalActivityLog).Sum(s => ((ChallengeKcalActivityLog)s).Kcal)
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
                    AccountChallengeProgress firstOrderedExercise = orderedAccountExercises.FirstOrDefault();
                    AccountChallengeProgress lastOrderedExercise = orderedAccountExercises.LastOrDefault();
                    if (firstOrderedExercise.TotalExercises > lastOrderedExercise.TotalExercises)
                        winnerAccountInExercises = firstOrderedExercise.Account;

                    AccountChallengeProgress firstOrderedKm = orderedAccountKm.FirstOrDefault();
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

        public async Task<ChallengeActivityLog> AddExerciseActivityAsync(Location myLocation, string challenge = null)
        {
            try
            {
                var activity = new ChallengeWorkoutActivityLog
                {
                    Account = _accountService.LoggedInAccount,
                    Challenge = challenge,
                    DatetTime = DateTime.Now,
                    Location = myLocation
                };
                ChallengeActivityLog newActivity = await AddUpdateActivityAsync(activity);
                return newActivity;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Models.Challenge.Challenge> IsExercisingInChallengeWorkoutPlaceAsync(Location myLocation)
        {
            if (!string.IsNullOrEmpty(_accountService.LoggedInAccount.ChallengeId))
            {
                var _challenge = await _challengeService.GetChallengeAsync(_accountService.LoggedInAccount.ChallengeId);

                var workoutLocation = _challenge.SelectedLocation;
                if (workoutLocation == null)
                    return null;
                double near = _locationService.DistanceBetweenPlaces(myLocation.Lng, myLocation.Lat, _challenge.SelectedLocation.Lng, _challenge.SelectedLocation.Lat);
                if (near > MinDistanceForWorkout)
                    return null;
                else
                    return _challenge;
            }
            return null;
        }
    }
}