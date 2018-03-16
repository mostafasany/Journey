using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Models.Storage;

namespace Abstractions.Services.Contracts
{
    public interface ILocalStorageService
    {
        Task OpenFolderAsync(string path);
        Task<List<LocalFile>> PickSingleFolderAndReturnFilesAsync();
    }
}