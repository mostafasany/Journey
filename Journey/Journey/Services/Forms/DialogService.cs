using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Core;

namespace Journey.Services.Forms
{
    public class DialogService : IDialogService
    {
        public string ErrorMessageBody { get; set; }
        public string ErrorMessageTitle { get; set; }
        public string NoInternetMessageBody { get; set; }
        public string NoInternetMessageTitle { get; set; }

        public async Task ShowMessageAsync(string content, string title)
        {
            //var answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
        }

        public async Task ShowGenericErrorMessageAsync(string content = "", string title = "")
        {
            //var answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
        }

        public async Task ShowNoInternetMessageAsync(string content = "", string title = "")
        {
            //var answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
        }

        public async Task ShowMessageAsync(string content, string title, IEnumerable<DialogCommand> dialogCommands)
        {
            //var answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
        }

        public async Task ShowToastNotificationAsync(string title, string content)
        {
            //var answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
        }
    }
}