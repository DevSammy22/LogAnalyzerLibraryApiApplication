using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class SearchLogsResponse
    {
        public bool Exists { get; set; }

        public string FilePath { get; set; }
    }
}
