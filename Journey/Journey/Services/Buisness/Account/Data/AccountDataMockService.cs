using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Abstractions.Contracts;
using Abstractions.Exceptions;
using Abstractions.Services.Contracts;
using Journey.Services.Buisness.Account.Dto;
using Journey.Services.Buisness.Account.Translators;
using Journey.Services.Buisness.Post.Data;
using Microsoft.WindowsAzure.MobileServices;

namespace Journey.Services.Buisness.Account.Data
{
    public class AccountDataMockService : IAccountDataService
    {
        private readonly ISerializerService _serializerService;

        public AccountDataMockService(ISerializerService serializerService) => _serializerService = serializerService;

        public async Task<Models.Account.Account> AddUpdateAccountAsync(Models.Account.Account account, bool add)
        {
            try
            {
                if (account == null)
                    return null;

                var assembly = typeof(PostDataMockService).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream("Journey.Services.Mocks.AccountMock.xml");
                string text;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                var azureAccountDto = _serializerService.DeserializeFromString<AzureAccount>(text);

                account = AccountDataTranslator.TranslateAccount(azureAccountDto);
                return account;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }


        public async Task<Models.Account.Account> GetAccountAsync(bool sync = false)
        {
            try
            {
                var assembly = typeof(PostDataMockService).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream("Journey.Services.Mocks.AccountMock.xml");
                string text;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                var azureAccountDto = _serializerService.DeserializeFromString<AzureAccount>(text);

                var accountDto = AccountDataTranslator.TranslateAccount(azureAccountDto);

                return accountDto;
            }
            catch (Exception)
            {
                //Means User not exists
                return null;
            }
        }


        public bool IsAccountAuthenticated()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<MobileServiceUser> AutehticateAsync()
        {
            try
            {
                return new MobileServiceUser("sid:b25963d532a96ad25414f36344f9c488")
                {
                    MobileServiceAuthenticationToken =
                        "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdGFibGVfc2lkIjoic2lkOjgzNzc3ZjQ0NWQ3MmRkODhiM2MyZjg1ZjhjYzVjZDhiIiwic3ViIjoic2lkOmIyNTk2M2Q1MzJhOTZhZDI1NDE0ZjM2MzQ0ZjljNDg4IiwiaWRwIjoiZmFjZWJvb2siLCJ2ZXIiOiIzIiwiaXNzIjoiaHR0cHM6Ly9qb3VybmV5Y2hhbGxlbmdlLmF6dXJld2Vic2l0ZXMubmV0LyIsImF1ZCI6Imh0dHBzOi8vam91cm5leWNoYWxsZW5nZS5henVyZXdlYnNpdGVzLm5ldC8iLCJleHAiOjE1MjUwMTM1MjQsIm5iZiI6MTUxOTgzMjY1NX0.KsGN1XlFt8lgpcOMcH3mbd4I4IJR-2RCOpjSCRJf26M"
                };
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }

        public async Task<List<Social>> MeAsync()
        {
            try
            {
                var assembly = typeof(PostDataMockService).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream("Journey.Services.Mocks.MeMock.xml");
                string text;
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                var socialInfo = _serializerService.DeserializeFromString<List<Social>>(text);
                return socialInfo;
            }
            catch (Exception ex)
            {
                throw new DataServiceException(ex);
            }
        }
    }
}