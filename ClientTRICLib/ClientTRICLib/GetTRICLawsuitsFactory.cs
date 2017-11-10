using ClientTRICLib.Common;

namespace ClientTRICLib
{
    /// <summary>
    /// Фабрика позволяет получить из Гелиос перечень дел из журнала дел.
    /// Как использовать фабрику - см. код юнит-теста на фабрику
    /// </summary>
    public class GetTRICLawsuitsFactory : ITRICServicesFactory
    {
        public GetTRICLawsuitsFactory(IServiceIntegrator client)
            : base(client)
        {
            
        }

        public override int OperationId
        {
            get { return (int)TRICMethods.GetTRICLawsuits; }
        }
    }
}
