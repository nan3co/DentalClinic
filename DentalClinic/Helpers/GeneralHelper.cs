using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DentalClinic.Helpers
{
    public class GeneralHelper
    {
        private static readonly Regex RegexStripDiacritics = new Regex(@"\p{IsCombiningDiacriticalMarks}+", RegexOptions.Compiled);
        public static string TripNonAsciiString(string s)
        {
            return Regex.Replace(s, @"[^\u0000-\u007F]+", string.Empty);
        }
        public static string ToAscii(string source, bool removeDoubleWhiteSpace = true)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            string result = StripDiacritics(source);
            if (removeDoubleWhiteSpace)
                return RemoveDoubleWhiteSpace(result);

            return result;
        }
        public static string StripDiacritics(string accented)
        {
            if (string.IsNullOrWhiteSpace(accented))
                return string.Empty;

            string strFormD = accented.Normalize(NormalizationForm.FormD);

            return RegexStripDiacritics.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string RemoveDoubleWhiteSpace(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;

            return RemoveDoubleWhiteChar(' ', source);
        }
        public static string RemoveDoubleWhiteChar(char c, string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;
            return source.Replace(c.ToString(), "ᵔᵕ").Replace("ᵕᵔ", "").Replace("ᵔᵕ", c.ToString()).Trim();
        }
    }
}
