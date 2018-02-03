using System;
using System.Threading.Tasks;

namespace Abstractions.Services.Contracts
{
    public interface IPopupService
    {
        Task<object> ShowPopup(string name, object obj);
        void RegisterPopup(string name, Type popup);
    }
}