using System;
using System.Threading.Tasks;

namespace Services.Core
{
    public interface IPopupService
    {
        Task<object> ShowPopup(string name, object obj);
        void RegisterPopup(string name, Type popup);
    }
}