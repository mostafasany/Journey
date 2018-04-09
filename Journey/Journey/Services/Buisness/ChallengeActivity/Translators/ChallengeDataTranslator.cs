using System;
using System.Collections.Generic;
using System.Globalization;
using Abstractions.Exceptions;
using Abstractions.Forms;
using Abstractions.Models;
using Journey.Models.Challenge;
using Journey.Services.Buisness.ChallengeActivity.Dto;
using Newtonsoft.Json;

namespace Journey.Services.Buisness.ChallengeActivity.Translators
{
    public class ChallengeActivityDataTranslator
    {
        private const int ChallengeKmActivityLogId = 1;
        private const int ChallengeWorkoutActivityLogId = 2;

        public static AzureChallengeActivity TranslateChallengeActivity(ChallengeActivityLog activityLog)
        {
            var postDto = new AzureChallengeActivity();
            if (activityLog != null)
            {
                postDto.Id = activityLog.Id;
                postDto.CreatedAt = activityLog.DatetTime;
                postDto.Account = activityLog.Account.Id;
                postDto.Challenge = activityLog.Challenge;
                if (activityLog is ChallengeKmActivityLog logKm)
                {
                    postDto.Type = ChallengeKmActivityLogId;
                    postDto.Activity = logKm.KM.ToString(CultureInfo.InvariantCulture);
                }
                else if (activityLog is ChallengeWorkoutActivityLog logWorkout)
                {
                    postDto.Type = ChallengeWorkoutActivityLogId;
                    postDto.Activity = JsonConvert.SerializeObject(logWorkout.Location);
                }
            }

            return postDto;
        }

        public static ChallengeActivityLog TranslateChallengeActivity(AzureChallengeActivity actvityLog)
        {
            if (actvityLog == null) return null;

            switch (actvityLog.Type)
            {
                case 1:
                {
                    var activity = new ChallengeKmActivityLog
                    {
                        Id = actvityLog.Id,
                        DatetTime = actvityLog.CreatedAt,
                        Account = new Models.Account.Account
                        {
                            Id = actvityLog.Account,
                            FirstName = actvityLog.Fname,
                            LastName = actvityLog.Lname,
                            Image = new Media {Path = actvityLog.Profile}
                        },
                        KM = double.Parse(actvityLog.Activity)
                    };
                    return activity;
                }
                case 2:
                {
                    var activity = new ChallengeWorkoutActivityLog
                    {
                        Id = actvityLog.Id,
                        DatetTime = actvityLog.CreatedAt,
                        Account = new Models.Account.Account
                        {
                            Id = actvityLog.Account,
                            FirstName = actvityLog.Fname,
                            LastName = actvityLog.Lname,
                            Image = new Media {Path = actvityLog.Profile}
                        },
                        Location = JsonConvert.DeserializeObject<Location>(actvityLog.Activity)
                    };
                    return activity;
                }
            }

            return null;
        }

        public static List<ChallengeActivityLog> TranslateChallengeActivityList(List<AzureChallengeActivity> actvityLog)
        {
            try
            {
                var logsDto = new List<ChallengeActivityLog>();
                foreach (AzureChallengeActivity log in actvityLog)
                {
                    ChallengeActivityLog logDto = TranslateChallengeActivity(log);
                    logsDto.Add(logDto);
                }

                return logsDto;
            }
            catch (Exception ex)
            {
                throw new TranslationFailedException("Post", ex);
            }
        }
    }
}