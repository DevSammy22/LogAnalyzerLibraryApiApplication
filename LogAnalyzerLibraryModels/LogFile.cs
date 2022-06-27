using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class LogFile
    {
        public int LogFileId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime Date { get; set; }
        public FileInfo File { get; set; }
        public string FileLocation { get; set; }
        public bool Exists { get; set; }
        public string FilePath { get; set; }
    }
}
