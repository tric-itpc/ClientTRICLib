using ClientTRICLib.wsIntegration;

namespace ClientTRICLib.Common
{
    public static class ServiceIntegratorClientFactory
    {
        public static ServiceIntegratorClient Make(string url)
        {
            ServiceIntegrator client = new ServiceIntegrator(url);
            return new ServiceIntegratorClient(client);
        }   
    }
}
