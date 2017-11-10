using System;

namespace ClientTRICLib.Common
{
    public static class IntegerExtensions
    {
        /// <summary>
        /// Преобразование типа "Месяц-ТРИЦ" в Дату
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime MonthToDate(this int month)
        {
            DateTime res;
            res = Convert.ToDateTime("01.01.1900").AddMonths(month + 1163);
            return res;
        }
    }
}
