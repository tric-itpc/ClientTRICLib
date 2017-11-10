using ClientTRICLib.Common;
using System;
using System.Data;
using System.Linq;

namespace ClientTRICLib
{
    public abstract class ITRICServicesFactory
    {
        private IServiceIntegrator client;
        
        public abstract int OperationId { get; }

        public ITRICServicesFactory(IServiceIntegrator client)
        {
            this.client = client;
        }

        public DataSet Make(object[] parameters, string ticket)
        {
            try
            {
                var command = new OperationByIdCommand(this.client, this.OperationId, parameters.ToList(), ticket);
                command.Execute();
                return command.ResultDataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
