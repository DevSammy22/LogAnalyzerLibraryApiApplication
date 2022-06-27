using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class SearchPerDirectory
    {
        public string FileName { get; set; }
        public IEnumerable<string> Directories { get; set; }
    }
}
