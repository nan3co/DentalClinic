using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DentalClinic.Helpers
{
    public class GeneralHelper
    {
        public static string TripNonAsciiString(string s)
        {
            return Regex.Replace(s, @"[^\u0000-\u007F]+", string.Empty);
        }
    }
}
