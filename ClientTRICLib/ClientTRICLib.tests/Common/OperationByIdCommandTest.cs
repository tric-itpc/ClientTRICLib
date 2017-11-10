using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using System.Collections.Generic;
using ClientTRICLib.Common;
using ClientTRICLib.wsIntegration;

namespace ClientTRICLib.tests.Common
{
    [TestClass]
    public class OperationByIdCommandTest
    {
        private OperationByIdCommand command;
        private OperationByIdCommand commandException;
        private DataSet dataSet;
        private Mock<IServiceIntegrator> mockClient;

        [TestInitialize]
        public void SetUp()
        {
            this.mockClient = new Mock<IServiceIntegrator>();
            this.dataSet = new DataSet();
            List<object> values = new List<object>();

            int typeLawsuit = (int)TypeLawsuit.LawsuitCourtOrder;
            values.Add(typeLawsuit);

            int operationId = 11;
            int[] operations = new int[1] { operationId };

            int operationExceptionId = 1;
            int[] operationsException = new int[1] { operationExceptionId };

            List<OperationParam> operationParams = new List<OperationParam>
            {
                new OperationParam
                {
                    OperationId = operationId,
                    NameParam = "val1",
                    ValueType = ClientTRICLib.wsIntegration.SqlDbType.Int
                }
            };

            string ticket = "anonym";
            mockClient.Setup(x => x.GetOperationParams(operationId))
                .Returns(operationParams.ToArray());
            
            mockClient.Setup(x => x.OperationByIdInvoke(ticket, operations, It.Is<OperationValue[]>(w => w[0].Value == typeLawsuit.ToString())))
                .Returns(this.dataSet);

            mockClient.Setup(x => x.OperationByIdInvoke(ticket, operationsException, It.IsAny<OperationValue[]>()))
                .Throws(new Exception("Error!!!"));
            
            this.command = new OperationByIdCommand(this.mockClient.Object, operationId, values, ticket);
            this.commandException = new OperationByIdCommand(this.mockClient.Object, operationExceptionId, values, ticket);
        }

        [TestMethod]
        public void OperationByIdCommand_IsInstatce()
        {
            Assert.IsInstanceOfType(this.command, typeof(ICommand));
        }

        [TestMethod]
        public void OperationByIdCommand_Execute_Success()
        {
            this.command.Execute();

            Assert.AreEqual(this.dataSet, this.command.ResultDataSet);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Error!!!")]
        public void OperationByIdCommand_Execute_WithException()
        {
            this.commandException.Execute();

            Assert.IsNull(this.commandException.ResultDataSet);
        }
    }
}
