using ClientTRICLib.wsIntegration;
using System.Data;

namespace ClientTRICLib.Common
{
    public interface IServiceIntegrator
    {
        DataSet OperationByIdInvoke(string ticket, int[] operationIDKeys, OperationValue[] requestParamValues);
        OperationParam[] GetOperationParams(int operationId);
        string GetAuthorizationTicket(string login, string password);
    }
}
