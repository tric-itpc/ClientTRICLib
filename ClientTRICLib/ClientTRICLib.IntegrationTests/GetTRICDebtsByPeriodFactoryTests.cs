using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using ClientTRICLib.Common;
using System.Linq;
using System.Collections.Generic;

namespace ClientTRICLib.IntegrationTests
{
    [TestClass]
    public class GetTRICDebtsByPeriodFactoryTests
    {
        private GetTRICDebtsByPeriodFactory factory;
        private ServiceIntegratorClient client;
        private string ticket;
        private string columnNamesInResponse;

        [TestInitialize]
        public void Setup()
        {
            string user = IdentityCredential.User;
            string password = IdentityCredential.Password;

            this.columnNamesInResponse = "[cod_pl],[for_period],[s_money],[address],[FIO],[i_house],[d_born]";

            this.client = ServiceIntegratorClientFactory.Make(WebServiceUrl.Url);

            this.ticket = this.client.GetAuthorizationTicket(user, password);

            this.factory = new GetTRICDebtsByPeriodFactory(client);
        }

        [TestMethod]
        public void GetTRICDebtsByPeriodFactory_Make()
        {
            int i_owner = 1032; // код поставщика, выдается администратором системы

            var xmlRoot = new XmlBuilder("Accounts");

            var accountElement = new XmlBuilder("Account");

            accountElement.AddAttribute("accountID", 11816105);
            string b_period = new DateTime(2004, 12, 1).ToString("dd.MM.yyyy");
            string e_period = new DateTime(2016, 10, 1).ToString("dd.MM.yyyy");
            accountElement.AddAttribute("b_period", b_period);
            accountElement.AddAttribute("e_period", e_period);

            xmlRoot.AddElement(accountElement.Build());

            string xmlAccounts = xmlRoot.Build().ToString();
            /* ИЛИ так:
            string xmlAccounts = @"<Accounts>
		        <Account accountID=""11816105"" b_period=""01.12.2014"" e_period=""01.10.2016"" />
	        </Accounts>";
            */

            var response = this.factory.Make(new object[] { i_owner, xmlAccounts }, ticket);

            Assert.IsInstanceOfType(response, typeof(DataSet));
            Assert.AreEqual(1, response.Tables.Count);
            Assert.AreEqual(7, response.Tables[0].Columns.Count);
            Assert.IsTrue(response.Tables[0].Rows.Count > 0);

            int colNum = 0;
            string[] columns = columnNamesInResponse.Replace("[", "").Replace("]", "").Split(new char[] { ',' });

            response.Tables[0].Columns.Cast<DataColumn>().ToList()
                .ForEach(x =>
                {
                    Assert.AreEqual(columns[colNum], x.ColumnName);
                    colNum++;
                });

            IEnumerable<DataRow> rowsTop10 = response.Tables[0].Rows.Cast<DataRow>().Take(10);

            foreach (DataRow row in rowsTop10)
            {
                Assert.IsTrue((int)row["cod_pl"] > 0, "Лицевой счет не обнаружен");
                Assert.IsTrue((int)row["for_period"] > 0, "Код периода не обнаружен");

                decimal sumSaldo;
                bool isDecimal = Decimal.TryParse(row["s_money"].ToString(), out sumSaldo);
                Assert.IsTrue(isDecimal, "Сумма задолженности не обнаружена");
                
                Assert.IsTrue(row["address"].ToString() != "", "Адрес не обнаружена");
                Assert.IsTrue(row["FIO"].ToString() != "", "Фамилия не обнаружена");
                Assert.IsTrue((int)row["i_house"] > 0, "Код дома не обнаружен");
                Assert.IsTrue(row["d_born"].ToString() != "", "Дата рождения не обнаружена");
            }
        }
    }
}
