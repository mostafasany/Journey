﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Challenge;
using Journey.Services.Azure;
using Journey.Services.Buisness.ChallengeActivity.Dto;
using Journey.Services.Buisness.ChallengeActivity.Translators;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

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

                var logModel = ChallengeActivityDataTranslator.TranslateChallengeActivity(logDto);
                logModel.Account = log.Account;
                logModel.Mine = true;
                return logModel;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<List<ChallengeActivityLog>> GetAccountActivitiesAsync()
        {
            try
            {
                string api = "AccountActivity";
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

        public async Task<ChallengeActivityLog> UpdateActivityAsync(ChallengeActivityLog log)
        {
            try
            {
                if (log == null)
                    return null;

                AzureChallengeActivity logDto = ChallengeActivityDataTranslator.TranslateChallengeActivity(log);
                JObject jo = new JObject();
                jo.Add(nameof(logDto.Id).ToLower(), logDto.Id);
                jo.Add(nameof(logDto.Activity).ToLower(), logDto.Activity);
                await _azureChallengeActivity.UpdateAsync(jo);

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

                await _azureChallengeActivity.DeleteAsync(new AzureChallengeActivity { Id = log.Id });

                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<List<ChallengeActivityLog>> GetChallengeActivitiesAsync(string challengeId)
        {
            try
            {
                string api = string.Format("ChallengeActivity?challenge={0}", challengeId);
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
    }
}