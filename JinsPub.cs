
using System;
using System.Text;
namespace Jinsftpweb
{
    public class JinsPub
    {
        public static string OrdID = "";
        public static string DbName = "HKOERPCONNECTION";

        public static string ID25
        {
            get
            {
                return GenerateRandomID25();
            }
        }

        private static char[] constant10 =
        {
        '0','1','2','3','4','5','6','7','8','9'
        };

        private static char[] constant62 =
        {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
        };

        private static string GenerateRandom(int Length)
        {
            StringBuilder newRandom = new StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant62[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        private static string GenerateRandomID25()
        {
            string id25;
            string strYear = DateTime.Now.Year.ToString();
            string strMonth = "0" + DateTime.Now.Month.ToString();
            string strDay = "0" + DateTime.Now.Day.ToString();
            string strHour = "0" + DateTime.Now.Hour.ToString();
            string strMinute = "0" + DateTime.Now.Minute.ToString();
            string strSecond = "0" + DateTime.Now.Second.ToString();
            string strMSecond = "00" + DateTime.Now.Millisecond.ToString();

            StringBuilder newRandom = new StringBuilder(10);
            Random rd = new Random();
            for (int i = 0; i < 10; i++)
            {
                newRandom.Append(constant10[rd.Next(10)]);
            }
            id25 = strYear.GetRightStr(2) +
                strMonth.GetRightStr(2) +
                strDay.GetRightStr(2) +
                strHour.GetRightStr(2) +
                strMinute.GetRightStr(2) +
                strSecond.GetRightStr(2) +
                strMSecond.GetRightStr(3) + newRandom.ToString();
            return id25;
        }

    }
}