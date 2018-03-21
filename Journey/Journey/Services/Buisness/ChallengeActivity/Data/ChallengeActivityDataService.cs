using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Abstractions.Models;
using Journey.Models.Challenge;
using Journey.Services.Azure;
using Journey.Services.Buisness.ChallengeActivity.Dto;
using Journey.Services.Buisness.ChallengeActivity.Translators;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.ChallengeActivity.Data
{
    public class ChallengeActivityDataService : IChallengeActivityDataService
    {
        private readonly IMobileServiceTable<AzureChallengeActivity> _azureChallengeActivity;
        private readonly MobileServiceClient _client;

        public ChallengeActivityDataService(IAzureService azureService)
        {
            _client = azureService.CreateOrGetAzureClient();
            _azureChallengeActivity = _client.GetTable<AzureChallengeActivity>();
        }

        public async Task<ChallengeActivityLog> AddActivityAsync(ChallengeActivityLog log)
        {
            try
            {
                if (log == null)
                    return null;

                AzureChallengeActivity logDto = ChallengeActivityDataTranslator.TranslateChallengeActivity(log);
                await _azureChallengeActivity.InsertAsync(logDto);

                log = ChallengeActivityDataTranslator.TranslateChallengeActivity(logDto);
                return log;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<bool> DeleteActivityAsync(ChallengeActivityLog log)
        {
            try
            {
                if (log == null)
                    return false;

                await _azureChallengeActivity.DeleteAsync(new AzureChallengeActivity {Id = log.Id});

                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<List<ChallengeActivityLog>> GetActivitsAsync(string challengeId, int page, int size)
        {
            try
            {
                MockData2();
                string api = string.Format("ChallengeActivity?size={0}&page={1}&challenge={2}", size, page, challengeId);
                List<AzureChallengeActivity> logs = await _client.InvokeApiAsync<List<AzureChallengeActivity>>(api, HttpMethod.Get, null);

                if (logs == null || logs.Count == 0)
                    return null;
                List<ChallengeActivityLog> logsDto = ChallengeActivityDataTranslator.TranslateChallengeActivityList(logs);
                logsDto.Where(a => a.Account.Id == _client.CurrentUser.UserId).ToList().ForEach(c => c.Mine = true);
                return logsDto;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        async void MockData()
        {
            var account1 = new Models.Account.Account()
            {
                FirstName = "Mostafa",
                LastName = "Khodeir",
                Id = "sid:b25963d532a96ad25414f36344f9c488"
            };
            var account2 = new Models.Account.Account()
            {
                FirstName = "Heba",
                LastName = "EL Leithy",
                Id = "sid:387799b87d537c0376e4ce5045de7a18"
            };
            var status = new List<ChallengeActivityLog>
            {
                new ChallengeWorkoutActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1),
                    Location = new Location{Name = "FitX"},
                    Mine=true,
                    Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(1),
                    Location = new Location{Name = "FitX"},   Mine=true,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(1),
                    KM = 2,   Mine=true,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(2),
                    Location = new Location{Name = "FitX"},    Mine=true,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(2),Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c",
                    KM = 5,   Mine=true,
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(3),
                    Location = new Location{Name = "FitX"},   Mine=true,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(3),Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c",
                    KM = 8,   Mine=true,
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1),
                    Location = new Location{Name = "FitX"},    Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1),
                    KM = 6,  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(1),
                    Location = new Location{Name = "FitX"},    Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(1),
                    KM = 2,  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(2),
                    Location = new Location{Name = "FitX"},  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(2),
                    KM = 5,  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(3),
                    Location = new Location{Name = "FitX"},   Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(3),
                    KM = 8,  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },

            };

            foreach (var sta in status)
            {
                await AddActivityAsync(sta);
            }
        }

        async void MockData2()
        {
            var account1 = new Models.Account.Account()
            {
                FirstName = "Mostafa",
                LastName = "Khodeir",
                Id = "sid:b25963d532a96ad25414f36344f9c488"
            };
            var account2 = new Models.Account.Account()
            {
                FirstName = "Heba",
                LastName = "EL Leithy",
                Id = "sid:387799b87d537c0376e4ce5045de7a18"
            };
            var status = new List<ChallengeActivityLog>
            {
                new ChallengeWorkoutActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1),
                    Location = new Location{Name = "FitX"},
                    Mine=true,
                    Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeWorkoutActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(1),
                    Location = new Location{Name = "FitX"},   Mine=true,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account1,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(2),
                    KM = 4,  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },

                new ChallengeWorkoutActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1),
                    Location = new Location{Name = "Golds Gym"},   Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(1),
                    KM = 8,  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
                new ChallengeKmActivityLog
                {
                    Account=account2,
                    DatetTime = DateTime.Now.AddMonths(1).AddDays(2),
                    KM = 6,  Mine=false,Challenge = "10a4ad4c-38a8-4be5-890b-317aacc0e27c"
                },
            };

            foreach (var sta in status)
            {
                await AddActivityAsync(sta);
            }
        }
    }
}