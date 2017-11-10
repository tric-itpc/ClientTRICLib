namespace ClientTRICLib.Common
{
    /// <summary>
    /// Виды запросов данных из системы Гелиос(ТРИЦ)
    /// </summary>
    public enum TRICMethods
    {
        /// <summary>
        /// Перечень дел из журнала Гелиос
        /// </summary>
        GetTRICLawsuits = 11,
        /// <summary>
        /// Возвращает зарегистрированных граждан по указанному лс и периоду, 
        /// возможно получение сразу по нескольким лицевым счетам
        /// </summary>
        GetTRICLivings = 12,
        /// <summary>
        /// Возвращает задолженность одной суммой
        /// </summary>
        GetTRICDebtsTotal = 13,
        /// <summary>
        /// Возвращает задолженность за период
        /// </summary>
        GetTRICDebtsByPeriod = 14,
        /// <summary>
        /// Возвращает задолженность за период в разрезе услуг
        /// </summary>
        GetTRICDebtsByServices = 15
    }
}
