using System;

namespace Abstractions.Services.Contracts
{
    public interface IAlarmService
    {
        void ShowNotification(string title, string body, DateTime startDate, DateTime endDate, string soundPath);
    }
}