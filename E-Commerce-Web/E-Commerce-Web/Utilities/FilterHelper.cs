using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_Web.Utilities
{
    public class FilterHelper
    {
        public static readonly string[] BadWords = new string[]
        {
            "bậy",
            "tục",
            "chửi",
            "ngu",
            "dốt",
            "đần",
            "điên",
            "đồ ngu",
            "thằng ngu",
            "ngu ngốc",
            "ngu xuẩn",
            "câm",
            "mày",
            "tao",
            "đồ chết tiệt",
            "đồ khốn",
            "đồ tồi",
            "mẹ kiếp",
            "chết tiệt",
            "xàm",
            "bậy bạ",
            "nhảm nhí",
            "vớ vẩn",
            "xạo",
            "xạo ke",
            "nói xạo",
            "bốc phét",
            "xí gạt",
            "nói láo",
            "dối trá",
            "xỏ lá",
            "lừa đảo"
        };
        public static string FilterProhibitedWordsWithoutRegex(string content)
        {
            var prohibitedWords = new HashSet<string>(BadWords);

            foreach (var word in prohibitedWords)
            {
                string replacement = new string('*', word.Length);

                int index = -1;
                while ((index = content.IndexOf(word, index + 1, StringComparison.OrdinalIgnoreCase)) != -1)
                {
                    content = content.Substring(0, index) + replacement + content.Substring(index + word.Length);
                }
            }

            return content;
        }

    }
}