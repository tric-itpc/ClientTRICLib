using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Linq;
using ClientTRICLib.Common;
using ClientTRICLib.wsIntegration;
using ClientTRICLib.Common;

namespace ClientTRICLib.IntegrationTests
{
    [TestClass]
    public class GetTRICDebtsTotalFactoryTests
    {
        private GetTRICDebtsTotalFactory factory;
        private ServiceIntegratorClient client;
        private string ticket;
        private string columnNamesInResponse;

        [TestInitialize]
        public void Setup()
        {
            string user = IdentityCredential.User;
            string password = IdentityCredential.Password;

            this.columnNamesInResponse = "[AccountID],[PeriodStr],[sumSaldo]";

            this.client = ServiceIntegratorClientFactory.Make(WebServiceUrl.Url);
            
            this.ticket = this.client.GetAuthorizationTicket(user, password);

            this.factory = new GetTRICDebtsTotalFactory(client);
        }

        [TestMethod]
        public void GetTRICDebtsTotalFactory_Make()
        {
            string i_owner = "1032"; // код поставщика: "Тепло Тюмени - филиал ПАО СУЭНКО"
            int i_lschet = 11816105; // номер лицевого счета

            var response = this.factory.Make(new object[] { i_owner, i_lschet }, ticket);

            Assert.IsInstanceOfType(response, typeof(DataSet));
            Assert.AreEqual(1, response.Tables.Count);
            Assert.AreEqual(3, response.Tables[0].Columns.Count);
            Assert.IsTrue(response.Tables[0].Rows.Count == 1);

            int colNum = 0;
            string[] columns = columnNamesInResponse.Replace("[", "").Replace("]", "").Split(new char[] { ',' });

            response.Tables[0].Columns.Cast<DataColumn>().ToList()
                .ForEach(x =>
                {
                    Assert.AreEqual(columns[colNum], x.ColumnName);
                    colNum++;
                });

            Assert.AreEqual(i_lschet, (int)response.Tables[0].Rows[0]["AccountID"]);

            string curPeriodMinusOneMonth = DateTime.Now.AddMonths(-1).ToString("01.MM.yyyy");

            Assert.AreEqual(curPeriodMinusOneMonth, response.Tables[0].Rows[0]["PeriodStr"]);

            decimal sumSaldo;
            bool isDecimal = Decimal.TryParse(response.Tables[0].Rows[0]["SumSaldo"].ToString(), out sumSaldo);
            Assert.IsTrue(isDecimal);
        }
    }
}
