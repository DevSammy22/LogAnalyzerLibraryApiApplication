using LogAnalyzerLibraryModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogAnalyzerLibraryCore.Service
{
    public interface ILibraryAnalyzer
    {
        Task ArchiveLogsFromPeriodAsync(ArchiveModel archiveModel);
        Task<int> CountAllDuplicationsAsync(Request request);
        Task<long> CountDuplicatedErrorsAsync(Request request);
        Task<long> CountUniqueErrorsAsync(Request request);
        Task<Response<string>> DeleteArchiveFromPeriodAsync(string path, string dateRange);
        Response<string> DeleteLogsFromPeriod(string directoryPath, string dateRange);
        string Format(string data);
        LogFile GetFile(string fileLocation);
        LogFile SearchLogsInDirectories(BaseRequest request);
        IEnumerable<LogFile> SearchLogsPerDirectory(SearchPerDirectory searchPerDirectory);
        IEnumerable<LogFile> SearchLogsPerSize(SearchPerSize searchPerSize);
        long TotalLogsAvailbaleAsync(string directoryPath, string dateRange);
        Task<Response<string>> UploadLogToServerAsync(UploadRequest uploadRequest);
    }
}