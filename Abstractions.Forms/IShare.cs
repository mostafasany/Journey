using System.Collections.Generic;
using System.Threading.Tasks;
using Journey.Models;

namespace Journey.Services.Forms
{
    public interface IShare
    {
        Task Share(string subject, string message, List<Media> media);

       
    }
}