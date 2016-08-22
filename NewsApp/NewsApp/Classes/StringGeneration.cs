using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApp.Classes
{
    public class StringGeneration
    {
        public static string getString(int size)
        {
            string str = DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
            str = str.Substring(0, str.Length - 2);
            return str;
        }
    }
}