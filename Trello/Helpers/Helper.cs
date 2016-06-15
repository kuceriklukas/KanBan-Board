using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trello.Helpers
{
    public static class Helper
    {
        public static String Split(String username)
        {
            var splitted = username.Split(' ').Select(s => s[0]);
            return String.Join("", splitted);
        }
    }
}