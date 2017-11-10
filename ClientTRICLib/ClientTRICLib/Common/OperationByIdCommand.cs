using System;
using System.Collections.Generic;
using System.Data;
using ClientTRICLib.wsIntegration;

namespace ClientTRICLib.Common
{
    public class OperationByIdCommand: ICommand
    {
        private IServiceIntegrator client;
        private int operationId;
        private List<object> values;
        private string ticket;
        private DataSet dataSet;

        public DataSet ResultDataSet
        {
            get
            {
                return this.dataSet;
            }
        }

        public OperationByIdCommand(IServiceIntegrator client, int operationId, List<object> values, string ticket)
        {
            this.client = client;
            this.operationId = operationId;
            this.values = values;
            this.ticket = ticket;
        }

        private List<OperationValue> PrepareOperationValueArray()
        {
            var operationParams = this.client.GetOperationParams(this.operationId);
            List<OperationValue> paramValues = new List<OperationValue>();
            int i = 0;
            foreach (var item in operationParams)
            {
                paramValues.Add(
                new OperationValue
                {
                    OperationId = item.OperationId,
                    NameParam = item.NameParam,
                    ValueType = item.ValueType,
                    Value = values[i].ToString(),
                    OperationIdSpecified = true,
                    ValueTypeSpecified = true
                });
                i++;
            }
            return paramValues;
        }

        public void Execute()
        {
            try
            {
                int[] operationIDKeys = new int[1] { this.operationId };
                OperationValue[] requestParamValues = PrepareOperationValueArray().ToArray();
                this.dataSet = this.client.OperationByIdInvoke(ticket, operationIDKeys, requestParamValues);
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Login failed for user"))
                    throw new Exception("Неверный логин или пароль, учетные данные Вы можете получить у администратора Гелиос. Пропишите их в классе: IdentityCredential");
                else
                    throw;
            }
        }
    }
}