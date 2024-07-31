using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCU
{
    internal static class DateUtils
    {
        public readonly static string FormatDialogic = "yyyy-MM-dd HH:mm:ss";
        public readonly static string FormatOutput = "ddd, MMMM dd, HH'h'mm";

        public static DateTime ParseDateString(string dateString)
        {
            DateTime parsedDate = DateTime.ParseExact(dateString, FormatDialogic, null);
            return parsedDate;
        }

        public static string ConvertDateToString(DateTime date)
        {
            string dateString = date.ToString(FormatOutput);
            return dateString;
        }

        public static string ConvertDateFromDialogicSave(string dialogicSaveState)
        {
            DateTime parsedDate = ParseDateString(dialogicSaveState);
            string dateString = ConvertDateToString(parsedDate);
            return dateString;
        }
    }
}
