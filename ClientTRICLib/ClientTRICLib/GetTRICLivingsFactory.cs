using ClientTRICLib.Common;

namespace ClientTRICLib
{
    /// <summary>
    /// Фабрика позволяет получить из Гелиос перечень зарегистрированных граждан 
    /// по указанному лицевому счету и периоду
    /// Как использовать фабрику - см. код юнит-теста на фабрику
    /// </summary>
    public class GetTRICLivingsFactory : ITRICServicesFactory
    {
        public GetTRICLivingsFactory(IServiceIntegrator client)
            : base(client)
        {
            
        }

        public override int OperationId
        {
            get { return (int)TRICMethods.GetTRICLivings; }
        }
    }
}
