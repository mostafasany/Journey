﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Models;
using Journey.Models.Challenge;

namespace Journey.Services.Buisness.Challenge
{
    public interface IChallengeService
    {
        Task<Models.Challenge.Challenge> GetChallengeAsync(string challengeId);
        Task<Models.Challenge.Challenge> ApproveChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> EndChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> AddChallengeAsync(Models.Challenge.Challenge challenge);
        Task<Models.Challenge.Challenge> UpdateExerciseNumberAsync(string challengeId);
        Task<List<ObservableGroupCollection<AccountChallengeProgress>>> GetChallengePorgessAsync(string challengeId);
        Task<bool> CheckAccountHasChallengeAsync();
    }
}