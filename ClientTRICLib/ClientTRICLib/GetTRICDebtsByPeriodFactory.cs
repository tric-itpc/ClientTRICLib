using ClientTRICLib.Common;

namespace ClientTRICLib
{
    /// <summary>
    /// Фабрика получает из Гелиос информацию о задолженностях за период для указанного 
    /// кода поставщика и лицевого счета
    /// Как использовать фабрику - см. код юнит-теста на фабрику
    /// </summary>
    public class GetTRICDebtsByPeriodFactory : ITRICServicesFactory
    {
        public GetTRICDebtsByPeriodFactory(IServiceIntegrator client) : base(client) { }        

        public override int OperationId
        {
            get { return (int)TRICMethods.GetTRICDebtsByPeriod; }
        }
    }
}
