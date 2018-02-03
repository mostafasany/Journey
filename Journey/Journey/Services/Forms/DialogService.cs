﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Services;
using Services.Core;
using Unity;

namespace Journey.Services.Forms
{
    public class DialogService : IDialogService
    {
        private readonly IPageDialogService _pageDialogService;

        public DialogService(IUnityContainer container)
        {
            if (container.IsRegistered<IPageDialogService>())
                _pageDialogService = container.Resolve<IPageDialogService>();
        }

        public string ErrorMessageBody { get; set; }
        public string ErrorMessageTitle { get; set; }
        public string NoInternetMessageBody { get; set; }
        public string NoInternetMessageTitle { get; set; }

        public async Task ShowMessageAsync(string content, string title)
        {
            await _pageDialogService.DisplayAlertAsync(title, content, "Cancel");
        }

        public async Task ShowGenericErrorMessageAsync(string content = "", string title = "")
        {
            await _pageDialogService.DisplayAlertAsync(title, content, "Cancel");
        }

        public async Task ShowNoInternetMessageAsync(string content = "", string title = "")
        {
            await _pageDialogService.DisplayAlertAsync(title, content, "Cancel");
        }

        public async Task ShowMessageAsync(string content, string title, IEnumerable<DialogCommand> dialogCommands)
        {
            await _pageDialogService.DisplayActionSheetAsync(title,
                dialogCommands.Select(command => ActionSheetButton.CreateButton(command.Label, command.Invoked))
                    .ToArray());
        }

        public async Task ShowToastNotificationAsync(string title, string content)
        {
            await _pageDialogService.DisplayAlertAsync(title, content, "Cancel");
        }
    }
}