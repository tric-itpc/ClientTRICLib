using ClientTRICLib.Common;
using ClientTRICLib.wsIntegration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;

namespace ClientTRICLib.IntegrationTests
{
    /// <summary>
    /// Юнит-тест показывает, как получить из Гелиос перечень зарегистрированных граждан
    /// </summary>
    [TestClass]
    public class GetTRICLivingsFactoryTests
    {
        private GetTRICLivingsFactory factory;
        private ServiceIntegratorClient client;
        private string ticket;
        private string columnNamesInResponse;

        [TestInitialize]
        public void SetUp()
        {
            // пользователя и пароль можно получить у администратора Гелиос
            string user = IdentityCredential.User;
            string password = IdentityCredential.Password;

            this.columnNamesInResponse = "cod_pl,i_people,FIO,d_born,isMain,PlaceBorn,b_period,e_period,periodName";

            this.client = ServiceIntegratorClientFactory.Make(WebServiceUrl.Url);
            
            this.ticket = this.client.GetAuthorizationTicket(user, password);

            this.factory = new GetTRICLivingsFactory(client);
        }

        [TestMethod]
        public void GetTRICLivingsFactory_Make()
        {
            // 1 - по регистрации, 2 - по владельцу, 3 - совмещенный
            int typeSolidarityReg = 3;

            var xmlRoot = new XmlBuilder("Accounts");

            var accountElement  = new XmlBuilder("Account");

            accountElement.AddAttribute("accountID", 1191582);
            int b_period = new DateTime(2004, 1, 1).DateToMonth();
            int e_period = new DateTime(2017, 12, 1).DateToMonth();
            accountElement.AddAttribute("b_period", b_period);
            accountElement.AddAttribute("e_period", e_period);

            xmlRoot.AddElement(accountElement.Build());

            string xmlAccounts = xmlRoot.Build().ToString();

            var response = this.factory.Make(new object[] { xmlAccounts, typeSolidarityReg }, ticket);

            Assert.IsInstanceOfType(response, typeof(DataSet));
            Assert.AreEqual(1, response.Tables.Count);
            Assert.AreEqual(9, response.Tables[0].Columns.Count);
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
                Assert.IsTrue((int)row["i_people"] > 0, "Код жильца не обнаружен");
                Assert.IsTrue(row["FIO"].ToString() != "", "Фамилия не обнаружена");
                Assert.IsTrue(row["d_born"].ToString() != "", "Дата рождения не обнаружена");
                Assert.IsTrue(row["isMain"].ToString() != "", "Ответственный квартиросъемщик указан");
                Assert.IsTrue(row["PlaceBorn"].ToString() != "", "Место рождения не указано");
                Assert.IsTrue(row["b_period"].ToString() != "", "Дата начала не указана");
                Assert.IsFalse(row["e_period"].ToString() != "", "Дата окончания не пуста");
            }
        }
    }
}
