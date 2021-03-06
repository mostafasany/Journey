﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IDialogService
    {
        string ErrorMessageBody { get; set; }
        string ErrorMessageTitle { get; set; }
        string NoInternetMessageBody { get; set; }
        string NoInternetMessageTitle { get; set; }
        Task ShowGenericErrorMessageAsync(string content = "", string title = "");
        Task ShowMessageAsync(string content, string title);
        Task ShowMessageAsync(string content, string title, IEnumerable<DialogCommand> dialogCommands);
        Task ShowNoInternetMessageAsync(string content = "", string title = "");
        Task ShowToastNotificationAsync(string title, string content);
    }

    public class DialogCommand
    {
        public object Id { get; set; }

        public string Label { get; set; }

        public Action Invoked { get; set; }
    }
}