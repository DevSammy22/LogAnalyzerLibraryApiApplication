using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class Request
    {
        public string FilePath { get; set; }

        public string FileName { get; set; }

        public List<string> Directories { get; set; }
    }
}
