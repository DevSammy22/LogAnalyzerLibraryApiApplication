using Aspose.Zip;
using Aspose.Zip.Saving;
using LogAnalyzerLibraryCore.Helper;
using LogAnalyzerLibraryModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LogAnalyzerLibraryCore.Service
{
    public class LibraryAnalyzer : ILibraryAnalyzer
    {
        private readonly ILibraryHelper _libraryHelper;

        public LibraryAnalyzer(ILibraryHelper libraryHelper)
        {
            _libraryHelper = libraryHelper;
        }

        public LogFile SearchLogsInDirectories(BaseRequest request)
        {

            var result = request.FileName[^4..];
            if (result != ".log")
            {
                request.FileName += ".log";
            }

            for (int i = 0; i < request.Directories.Count; i++)
            {
                var directory = request.Directories[i];
                if (directory[1] != ':')
                {
                    directory = "C:\\" + directory;
                }
                DirectoryInfo dir = new DirectoryInfo(directory);

                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Name.Equals(request.FileName))
                    {
                        return new LogFile
                        {
                            Name = file.Name,
                            Size = file.Length,
                            FileLocation = file.DirectoryName,
                            Date = file.LastWriteTime,
                            Exists = true,
                            FilePath = file.FullName
                        };

                    }
                }
            }
            throw new ArgumentException("The file does not exist");
        }

        public async Task<long> CountUniqueErrorsAsync(Request request)
        {
            if (string.IsNullOrEmpty(request.FilePath))
            {
                var file = SearchLogsInDirectories(request);
                if (file.Exists)
                {
                    request.FilePath = file.FilePath;
                }
                else return 0;
            }

            var dataCount = await _libraryHelper.GroupLogsAsync(request.FilePath);
            int counter = 0;

            foreach (var log in dataCount)
            {
                if (log.Value == 1)
                {
                    counter++;
                }

            }
            return counter;
        }

        public async Task<long> CountDuplicatedErrorsAsync(Request request)
        {
            if (string.IsNullOrEmpty(request.FilePath))
            {
                var file = SearchLogsInDirectories(request);
                if (file.Exists)
                {
                    request.FilePath = file.FilePath;
                }
                else return 0;
            }

            var dataCount = await _libraryHelper.GroupLogsAsync(request.FilePath);
            int counter = 0;

            foreach (var log in dataCount)
            {
                if (log.Value > 1)
                {
                    counter++;
                }
            }
            return counter;
        }

        public async Task<int> CountAllDuplicationsAsync(Request request)
        {
            if (string.IsNullOrEmpty(request.FilePath))
            {
                var file = SearchLogsInDirectories(request);
                if (file.Exists)
                {
                    request.FilePath = file.FilePath;
                }
                else return 0;
            }
            var dataCount = await _libraryHelper.GroupLogsAsync(request.FilePath);
            int counter = 0;

            foreach (var log in dataCount)
            {
                if (log.Value > 1) counter += log.Value;
            }
            return counter;
        }

        public async Task<Response<string>> DeleteArchiveFromPeriodAsync(string path, string dateRange)
        {
            string temp = Path.GetTempFileName();

            using (StreamReader reader = new StreamReader(path))
            using (StreamWriter writer = new StreamWriter(temp))
            {
                string line = await reader.ReadLineAsync();

                while (line != null)
                {
                    line = line[..10];
                    if (line != dateRange)
                        await writer.WriteLineAsync(line);
                }
            }

            File.Delete(path);
            File.Move(temp, path);
            return new Response<string>()
            {
                Message = "Your archive is successfully deleted",
                Success = true
            };

        }
        public async Task ArchiveLogsFromPeriodAsync(ArchiveModel archiveModel)
        {
            string archiveName = string.Empty;
            if (archiveModel.FilePaths.Count < 1)
            {
                throw new ArgumentException("At least one filePath must be specified");
            }
            else
            {
                IFormatProvider culture = new CultureInfo("en-US", true);
                using (StreamReader reader = new StreamReader(archiveModel.FilePaths[0]))
                {
                    var line = await reader.ReadLineAsync();
                    line = line[..10];
                    reader.Close();
                    archiveModel.StartDate = Format(line);
                }

                using (StreamReader reader = new StreamReader(archiveModel.FilePaths[archiveModel.FilePaths.Count - 1]))
                {
                    var line = await reader.ReadLineAsync();
                    line = line[..10];
                    reader.Close();
                    archiveModel.EndDate = Format(line);
                }

                archiveName = String.Join('-', archiveModel.StartDate, archiveModel.EndDate);
            }

            using FileStream zipFile = File.Open($"{archiveName}.zip", FileMode.Create);
            // File to be added
            for (int i = 0; i < archiveModel.FilePaths.Count; i++)
            {
                using FileStream source1 = File.Open(archiveModel.FilePaths[i], FileMode.Open, FileAccess.Read);
                using var archive = new Archive(new ArchiveEntrySettings());
                // File added
                archive.CreateEntry(archiveModel.FilePaths[i], source1);

                // ZIP file
                archive.Save(zipFile);
            }
        }

        public async Task<Response<string>> UploadLogToServerAsync(UploadRequest uploadRequest)
        {
            HttpClient request = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(60)
            };
            MultipartFormDataContent formDataContent = new MultipartFormDataContent();
            using (FileStream fs = new FileStream(uploadRequest.filePath, mode: FileMode.Open))
            {
                using BufferedStream bs = new BufferedStream(fs);
                formDataContent.Add(new StreamContent(bs), "file", new FileInfo(uploadRequest.filePath).FullName);
                var response = await request.PostAsync(uploadRequest.fileUrl, formDataContent);
                string result = await response.Content.ReadAsStringAsync();
                fs.Close();
            }
            return new Response<string>()
            {
                Message = "Your log is successfully uploaded",
                Success = true
            };
        }

        public Response<string> DeleteLogsFromPeriod(string directoryPath, string dateRange)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Name.Contains(dateRange))
                {
                    file.Delete();
                }
            }
            return new Response<string>()
            {
                Success = true,
                Message = "Your log is successfully deleted"
            };
        }

        public long TotalLogsAvailbaleAsync(string directoryPath, string dateRange)
        {
            int count = 0;
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                if (file.Name.Contains(dateRange))
                {
                    count++;
                }
            }
            return count;
        }
        public IEnumerable<LogFile> SearchLogsPerSize(SearchPerSize searchPerSize)
        {
            if (searchPerSize.Directories.Count() < 1)
            {
                throw new ArgumentException("At least one directory must be specified");
            }

            if (searchPerSize.SizeFrom > searchPerSize.SizeTo)
            {
                throw new ArgumentException("FromSize cannot be greater than ToSize");
            }

            List<string> fileLocations = new List<string>();
            List<LogFile> logFiles = new List<LogFile>();
            foreach (string directory in searchPerSize.Directories)
            {
                if (!Directory.Exists(directory))
                {
                    throw new DirectoryNotFoundException($"{directory} not valid");
                }

                fileLocations.AddRange(Directory.GetFiles(directory));
            }

            foreach (string location in fileLocations)
            {
                LogFile file = GetFile(location);
                if (file.Size >= (searchPerSize.SizeFrom * 1000) && file.Size <= (searchPerSize.SizeTo * 1000))
                {
                    logFiles.Add(file);
                }

            }
            return logFiles;
        }

        public IEnumerable<LogFile> SearchLogsPerDirectory(SearchPerDirectory searchPerDirectory)
        {

            if (searchPerDirectory.Directories.Count() < 1)
            {
                throw new ArgumentException("At least one directoryPath must be specified");
            }

            List<string> fileLocations = new List<string>();
            List<LogFile> logFiles = new List<LogFile>();
            foreach (var dir in searchPerDirectory.Directories)
            {
                if (!Directory.Exists(dir))
                {
                    throw new DirectoryNotFoundException($"{dir} not valid");
                }

                fileLocations.AddRange(Directory.GetFiles(dir));
            }
            foreach (string location in fileLocations)
            {
                LogFile file = GetFile(location);
                if (file.Name.Equals(searchPerDirectory.FileName))
                {
                    logFiles.Add(file);
                }
            }

            return logFiles;
        }

        public string Format(string data)
        {
            var splitdata = data.Split('.');
            var newFormat = string.Join('_', splitdata);

            return newFormat;
        }

        public LogFile GetFile(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                throw new FileNotFoundException($"No file found in location: {fileLocation}");
            }

            var file = new FileInfo(fileLocation);
            if (file.Extension != ".log")
            {
                throw new FileLoadException("The selected file is not a log file");
            }

            return new LogFile
            {
                File = file,
                Name = file.Name,
                Size = file.Length,
                FileLocation = fileLocation,
                Date = file.LastWriteTime
            };
        }
    }
}
