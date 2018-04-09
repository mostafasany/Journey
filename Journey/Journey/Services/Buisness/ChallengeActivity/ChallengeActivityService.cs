using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Services.Buisness.Account;
using Journey.Services.Buisness.ChallengeActivity.Data;

namespace Journey.Services.Buisness.ChallengeActivity
{
    public class ChallengeActivityService : IChallengeActivityService
    {
        private readonly IAccountService _accountService;
        private readonly IChallengeActivityDataService _challengeActivityDataService;

        public ChallengeActivityService(IChallengeActivityDataService challengeActivityDataService, IAccountService accountService)
        {
            _accountService = accountService;
            _challengeActivityDataService = challengeActivityDataService;
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

        public async Task<List<ChallengeActivityLog>> GetActivitsAsync(string challengeId)
        {
            try
            {
                List<ChallengeActivityLog> logDto = await _challengeActivityDataService.GetActivitsAsync(challengeId);
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
                List<ChallengeActivityLog> challengeProgress = await _challengeActivityDataService.GetActivitsAsync(challengeId);
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
                                TotalKm = b.Where(e => e is ChallengeKmActivityLog).Sum(s => ((ChallengeKmActivityLog) s).KM),
                                TotalExercises = b.Count(e => e is ChallengeWorkoutActivityLog)
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
    }
}