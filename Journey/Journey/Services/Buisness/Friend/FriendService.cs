﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Exceptions;
using Journey.Models.Account;
using Tawasol.Services.Data;

namespace Tawasol.Services
{
    public class FriendService : IFriendService
    {
        IFriendDataService friendDataService;
        public FriendService(IFriendDataService _friendDataService)
        {
            friendDataService = _friendDataService;
        }
       
        public async Task<List<string>> FollowAsync(List<string> followerId)
        {
            try
            {
                var failureIds = await friendDataService.FollowAsync(followerId);
                return failureIds;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<bool> UnFollowAsync(string friendshipId)
        {
            try
            {
                var status = await friendDataService.UnFollowAsync(friendshipId);
                return status;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<Account> GetFriendAsync(string id)
        {
            try
            {
                var acc = await friendDataService.GetFriendAsync(id);
                return acc;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Account>> GetFriendsAsync(string name)
        {
            try
            {
                var friends = await friendDataService.GetFriendsAsync(name);
                return friends;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public async Task<List<Account>> GetInspiredListAsync()
        {
            try
            {
                //var gallery = await GetAccountGalleryAsync();
                List<Account> inpiredList = await GetFriendsAsync("");
                //inpiredList.Select(a => a.Status = "Achieve and celebrate your health goals").ToList();
                //inpiredList.Select(a => a.MediaList = gallery).ToList();


                return inpiredList;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


    }
}