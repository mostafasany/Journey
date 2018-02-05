using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Models.Storage;

namespace Abstractions.Services.Contracts
{
    public interface ILocalStorageService
    {
        Task<List<LocalFile>> PickSingleFolderAndReturnFilesAsync();

        Task OpenFolderAsync(string path);
    }
}