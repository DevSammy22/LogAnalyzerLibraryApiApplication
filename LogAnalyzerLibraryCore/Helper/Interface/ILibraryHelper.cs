using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogAnalyzerLibraryCore.Helper
{
    public interface ILibraryHelper
    {
        Task<List<string>> DataReaderAsync(string path);
        Task<Dictionary<string, int>> GroupLogsAsync(string path);
    }
}