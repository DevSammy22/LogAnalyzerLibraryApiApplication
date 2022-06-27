using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class ArchiveModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> FilePaths { get; set; }
    }
}
