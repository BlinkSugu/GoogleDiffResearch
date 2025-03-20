using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDiffResearch
{
    public enum InputType
    {
        Text,
        XML
    }
    public class GoogleCompareRequest
    {
        public string OldVersionFile { get; set; } = string.Empty;
        public string NewVersionFile { get; set; } = string.Empty;
        public InputType FileType { get; set; } = InputType.Text;
    }
    public class GoogleCompareResponse
    {
        public bool isServiceSuccessfull { get; set; }
        public string ResultDescription { get; set; } = string.Empty;
        public bool Result { get; set; }
        public string OutHtml { get; set; } = string.Empty;

    }
}
