using System;
using System.Collections.Generic;
using System.Text;

namespace LogAnalyzerLibraryModels
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
    }
}
