using System;

namespace ClientTRICLib.Common
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// преобразование Даты в тип "Месяц-ТРИЦ"
        /// </summary>
        /// <param name="fDate"></param>
        /// <returns></returns>
        public static Int32 DateToMonth(this DateTime fDate)
        {
            return (fDate.Year - 1997) * 12 + fDate.Month;
        }
    }
}
