using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class SearchPerSize
    {
        public long SizeFrom { get; set; }
        public long SizeTo { get; set; }
        public IEnumerable<string> Directories { get; set; }
    }
}
