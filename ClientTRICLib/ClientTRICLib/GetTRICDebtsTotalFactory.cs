using ClientTRICLib.Common;

namespace ClientTRICLib
{
    /// <summary>
    /// Фабрика получает из Гелиос информацию по общей текущей задолженности для указанного 
    /// кода поставщика и лицевого счета
    /// Как использовать фабрику - см. код юнит-теста на фабрику
    /// </summary>
    public class GetTRICDebtsTotalFactory : ITRICServicesFactory
    {
        public GetTRICDebtsTotalFactory(IServiceIntegrator client) : base(client) { }        

        public override int OperationId
        {
            get { return (int)TRICMethods.GetTRICDebtsTotal; }
        }
    }
}
