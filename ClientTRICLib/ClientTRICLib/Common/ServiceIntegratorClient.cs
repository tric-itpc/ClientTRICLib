using ClientTRICLib.wsIntegration;
using System.Data;

namespace ClientTRICLib.Common
{
    public class ServiceIntegratorClient : IServiceIntegrator
    {
        private ServiceIntegrator client;
        public ServiceIntegratorClient(ServiceIntegrator client)
        {
            this.client = client;
        }

        public DataSet OperationByIdInvoke(string ticket, int[] operationIDKeys, OperationValue[] requestParamValues)
        {
            return this.client.OperationByIdInvoke(ticket, operationIDKeys, requestParamValues);
        }

        public OperationParam[] GetOperationParams(int operationId)
        {
            return this.client.GetOperationParams(operationId, true);
        }


        public string GetAuthorizationTicket(string login, string password)
        {
            return this.client.GetAuthorizationTicket(login, password);
        }
    }
}
