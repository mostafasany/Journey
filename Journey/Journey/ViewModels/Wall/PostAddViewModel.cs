using System;
using System.Windows.Input;
using Abstractions.Services.Contracts;
using Journey.Models.Post;
using Prism.Commands;
using Unity;

namespace Journey.ViewModels.Wall
{
    public class PostAddViewModel : PostBaseViewModel
    {
        private readonly IDeepLinkingService _deepLinkingService;

        public PostAddViewModel(IUnityContainer container) : base(container)
        {
            _deepLinkingService = container.Resolve<IDeepLinkingService>();
        }

        #region OpenDeepLinkCommand

        private ICommand _openDeepLinkCommand;

        public ICommand OpenDeepLinkCommand => _openDeepLinkCommand ??
                                               (_openDeepLinkCommand =
                                                   new DelegateCommand(OpenDeepLink));

        private void OpenDeepLink()
        {
            try
            {
                if (Post == null)
                    return;
                var post = Post as PostAd;
                _deepLinkingService.ParseDeepLinkingAndExecute(post?.DeepLink);
            }
            catch (Exception ex)
            {
                ExceptionService.Handle(ex);
            }
        }

        #endregion
    }
}