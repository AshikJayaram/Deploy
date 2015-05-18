using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeployNowClient.Models
{
    public class FileDetailsModel
    {
        public string FileName { get; set; }

        public DateTime CreationTime { get; set; }

        public string Region { get; set; }
    }
}