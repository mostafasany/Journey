using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Storage;

namespace Services.Core
{
    public interface ILocalStorageService
    {
        Task<List<LocalFile>> PickSingleFolderAndReturnFilesAsync();

        Task OpenFolderAsync(string path);
    }
}