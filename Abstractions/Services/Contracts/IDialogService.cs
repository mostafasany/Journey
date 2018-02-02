using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Core
{
    public interface IDialogService
    {
        string ErrorMessageBody { get; set; }
        string ErrorMessageTitle { get; set; }
        string NoInternetMessageBody { get; set; }
        string NoInternetMessageTitle { get; set; }
        Task ShowMessageAsync(string content, string title);
        Task ShowGenericErrorMessageAsync(string content = "", string title = "");
        Task ShowNoInternetMessageAsync(string content = "", string title = "");
        Task ShowMessageAsync(string content, string title, IEnumerable<DialogCommand> dialogCommands);
        Task ShowToastNotificationAsync(string title, string content);
    }

    public class DialogCommand
    {
        public object Id { get; set; }

        public string Label { get; set; }

        public Action Invoked { get; set; }
    }
}