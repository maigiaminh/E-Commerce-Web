using E_Commerce_Web.Models.Result;
using iText.StyledXmlParser.Jsoup.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Models
{
    public class BaseResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<BaseError> Errors { get; set; } = new List<BaseError>();

        public void Set(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }
    }
}