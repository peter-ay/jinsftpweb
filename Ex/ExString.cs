

namespace System
{
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class ExString
    {
        public static string GetMyStr(this string str)
        {
            return str == null ? "" : str.Trim().ToUpper();
        }

        public static string GetCorrectXlsName(this string str)
        {
            str = str.Replace("/", "-");
            str = str.Replace(@"\", "-");
            str = str.Replace(@"?", "-");
            str = str.Replace(@"*", "-");
            str = str.Replace(@"[", "-");
            str = str.Replace(@"]", "-");
            return str == null ? "" : str.Trim().ToUpper();
        }

        public static string GetRightStr(this string str, int length)
        {
            string _Rs = str;
            try
            {
                _Rs = str.Substring(str.Length - length, length);
            }
            catch { }
            return _Rs;
        }

        public static string GetMyShortDateStr(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static bool GetBoolStr(this string str)
        {
            return Convert.ToBoolean((str.ToString().Trim() == "1" || str.ToString().Trim().ToLower() == "true") ? true : false);
        }

        public static int GetIntStr(this string str)
        {
            return Convert.ToInt32(Convert.ToDouble(str.ToString().Trim() == "" ? "0" : str));
        }

        public static decimal GetDecimalStr(this string str)
        {
            return Convert.ToDecimal(str.ToString().Trim() == "" ? "0" : str);
        }

        public static string GetPrismDecenterStr(this string str)
        {
            var rs = "";
            switch (str)
            {
                case "IN":
                    rs = "IN";
                    break;
                case "OT":
                    rs = "OUT";
                    break;
                case "UP":
                    rs = "UP";
                    break;
                case "DN":
                    rs = "DOWN";
                    break;
                default:
                    rs = "";
                    break;
            }
            return rs;
        }
    }
}