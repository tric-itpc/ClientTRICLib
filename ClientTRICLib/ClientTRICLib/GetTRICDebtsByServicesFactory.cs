using ClientTRICLib.Common;

namespace ClientTRICLib
{
    /// <summary>
    /// Фабрика получает из Гелиос информацию о задолженностях за период в разрезе услуг для указанного 
    /// кода поставщика, лицевого счета и периода
    /// Как использовать фабрику - см. код юнит-теста на фабрику
    /// </summary>
    public class GetTRICDebtsByServicesFactory : ITRICServicesFactory
    {
        public GetTRICDebtsByServicesFactory(IServiceIntegrator client) : base(client) { }        

        public override int OperationId
        {
            get { return (int)TRICMethods.GetTRICDebtsByServices; }
        }
    }
}
