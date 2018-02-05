using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Abstractions.Services.Contracts;
using Unity;

namespace Abstractions.Services
{
    public class ExceptionService : BaseService, IExceptionService
    {
        private readonly IDialogService _dialogService;

        public ExceptionService(IUnityContainer container, IDialogService dialogService,
            ILoggerService loggerService) : base(container)
        {
            _dialogService = dialogService;
            LoggerService = loggerService;
        }

        public void Handle(Exception ex, [CallerMemberName] string method = "",
            [CallerLineNumber] int line = -1,
            [CallerFilePath] string file = "")
        {
            var paramDictionary =
                new Dictionary<string, string>
                {
                    {nameof(CallerMemberNameAttribute), method},
                    {nameof(CallerLineNumberAttribute), line.ToString()},
                    {nameof(CallerFilePathAttribute), file}
                };
            LoggerService.LogException(ex, paramDictionary);
        }

        public void HandleAndShowDialog(Exception ex, string error = "", [CallerMemberName] string method = "",
            [CallerLineNumber] int line = -1,
            [CallerFilePath] string file = "")
        {
            var paramDictionary =
                new Dictionary<string, string>
                {
                    {nameof(CallerMemberNameAttribute), method},
                    {nameof(CallerLineNumberAttribute), line.ToString()},
                    {nameof(CallerFilePathAttribute), file}
                };
            LoggerService.LogException(ex, paramDictionary);
            _dialogService.ShowGenericErrorMessageAsync(string.IsNullOrEmpty(error) ? ex.Message : error);
        }
    }
}