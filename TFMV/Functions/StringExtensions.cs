using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFMV.Functions
{
    public static class StringExtensions
    {
        public static bool ContsInstv(this string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static bool Eq(this string strA, string strB)
        {
            return String.Equals(strA, strB, StringComparison.OrdinalIgnoreCase);
        }
    }
}
