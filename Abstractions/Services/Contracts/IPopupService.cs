using System;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IPopupService
    {
        void RegisterPopup(string name, Type popup);
        Task<object> ShowPopup(string name, object obj);
    }
}