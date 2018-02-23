using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstractions.Forms
{
    public interface IShare
    {
        Task Share(string subject, string message, List<Media> media);
    }
}