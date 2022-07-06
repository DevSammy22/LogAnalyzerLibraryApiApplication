using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class BaseRequest
    {
        public string FileName { get; set; }

        public List<string> Directories { get; set; }
    }
}
