using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientTRICLib.Common;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;

namespace ClientTRICLib.IntegrationTests
{
    [TestClass]
    public class GetTRICDebtsByServicesFactoryTests
    {
        private GetTRICDebtsByServicesFactory factory;
        private ServiceIntegratorClient client;
        private string ticket;
        private string columnNamesInResponse;

        [TestInitialize]
        public void Setup()
        {
            string user = IdentityCredential.User;
            string password = IdentityCredential.Password;

            this.columnNamesInResponse = "[i_lschet],[period],";
            this.columnNamesInResponse += "[sum_nach_11],[sum_odn_11],[sum_opl_11],";
            this.columnNamesInResponse += "[sum_nach_8],[sum_odn_8],[sum_opl_8],";
            this.columnNamesInResponse += "[sum_nach_10],[sum_odn_10],[sum_opl_10],";
            this.columnNamesInResponse += "[sum_nach_9],[sum_odn_9],[sum_opl_9],";
            this.columnNamesInResponse += "[sum_total_nach],[sum_total_opl],[sum_total],[orderby]";

            WSHttpBinding binding = new WSHttpBinding();
            binding.MaxReceivedMessageSize = 2147483647;
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            this.client = ServiceIntegratorClientFactory.Make(WebServiceUrl.Url);

            this.ticket = this.client.GetAuthorizationTicket(user, password);

            this.factory = new GetTRICDebtsByServicesFactory(client);
        }

        [TestMethod]
        public void GetTRICDebtsByServicesFactory_Make()
        {
            int i_owner = 1032; // код поставщика, выдается администратором системы

            var xmlRoot = new XmlBuilder("Accounts");

            var accountElement = new XmlBuilder("Account");

            accountElement.AddAttribute("i_lschet", 11789150);
            string b_period = new DateTime(2012, 10, 1).ToString("dd.MM.yyyy");
            string e_period = new DateTime(2017, 1, 31).ToString("dd.MM.yyyy");
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
            Assert.AreEqual(18, response.Tables[0].Columns.Count);
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
                Assert.IsTrue((int)row["i_lschet"] > 0, "Лицевой счет не обнаружен");
                Assert.IsTrue(row["period"].ToString() !="", "Период не обнаружен");

                Assert.IsTrue(IsDecimal(row, "sum_nach_11"), "Сумма начислено за отопление не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_odn_11"), "Сумма начислено отопление на ОДН не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_opl_11"), "Сумма оплат за отопление не обнаружена");

                Assert.IsTrue(IsDecimal(row, "sum_nach_8"), "Сумма начислено за ГВС не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_odn_8"), "Сумма начислено ГВС на ОДН не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_opl_8"), "Сумма оплат за ГВС не обнаружена");

                Assert.IsTrue(IsDecimal(row, "sum_nach_10"), "Сумма начислено за ХВС не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_odn_10"), "Сумма начислено ХВС на ОДН не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_opl_10"), "Сумма оплат за ХВС не обнаружена");

                Assert.IsTrue(IsDecimal(row, "sum_nach_9"), "Сумма начислено за канализование не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_odn_9"), "Сумма начислено канализование на ОДН не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_opl_9"), "Сумма оплат за канализование не обнаружена");

                Assert.IsTrue(IsDecimal(row, "sum_total_nach"), "Сумма итого начислено не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_total_opl"), "Сумма итого оплачено не обнаружена");
                Assert.IsTrue(IsDecimal(row, "sum_total"), "Сумма итого задолженность не обнаружена");
            }
        }

        private static bool IsDecimal(DataRow row, string fieldName)
        {
            decimal sumSaldo;
            bool isDecimal = Decimal.TryParse(row[fieldName].ToString(), out sumSaldo);
            return isDecimal;
        }
    }
}
