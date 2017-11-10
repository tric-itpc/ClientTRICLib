using ClientTRICLib.Common;
using ClientTRICLib.wsIntegration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;

namespace ClientTRICLib.IntegrationTests
{
    /// <summary>
    /// Юнит-тест показывает, как получить из Гелиос перечень дел из журнала дел
    /// </summary>
    [TestClass]
    public class GetTRICLawsuitsFactoryTests
    {
        private GetTRICLawsuitsFactory factory;
        private ServiceIntegratorClient client;
        private string ticket;
        private string columnNamesInResponse;

        [TestInitialize]
        public void SetUp()
        {
            // пользователя и пароль можно получить у администратора Гелиос
            string user = IdentityCredential.User;
            string password = IdentityCredential.Password; 

            this.columnNamesInResponse = "[ID],[AccountID],[NumberID],[Address],[FIO],[DateBorn],[SumSaldo],[SumTax],[B_period],[E_period]";
            this.columnNamesInResponse += ",[CreateDate],[Status],[UserName],[SumPaySaldo],[SumPayTax],[SumPeni],[SumPayPeni],[ServiceGroups],[SumActualDebt],[OrderNumber]";

            this.client = ServiceIntegratorClientFactory.Make(WebServiceUrl.Url);
            
            this.ticket = this.client.GetAuthorizationTicket(user, password);

            this.factory = new GetTRICLawsuitsFactory(client);
        }

        [TestMethod]
        public void GetTRICLawsuitsFactory_Make()
        {
            int typeLawsuit = (int)TypeLawsuit.LawsuitCourtOrder;
            var response = this.factory.Make(new object[] { typeLawsuit }, ticket);
            
            Assert.IsInstanceOfType(response, typeof(DataSet));
            Assert.AreEqual(1, response.Tables.Count);
            Assert.AreEqual(20, response.Tables[0].Columns.Count);
            Assert.IsTrue(response.Tables[0].Rows.Count > 0);

            int colNum = 0;
            string[] columns = columnNamesInResponse.Replace("[", "").Replace("]", "").Split(new char[] { ',' });

            response.Tables[0].Columns.Cast<DataColumn>().ToList()
                .ForEach(x => {
                    Assert.AreEqual(columns[colNum], x.ColumnName);
                    colNum++;
                });

            IEnumerable<DataRow> rowsTop10 = response.Tables[0].Rows.Cast<DataRow>().Take(10);

            foreach (DataRow row in rowsTop10)
	        {
                Assert.IsTrue((int)row["ID"] > 0, "Идентификатор дела не обнаружен");
                Assert.IsTrue(row["AccountID"].ToString() != "", "Номер лицевого счета не обнаружен");
                Assert.IsTrue(row["Address"].ToString() != "", "Номер лицевого счета не обнаружен");
                Assert.IsTrue(row["FIO"].ToString() != "", "Ф.И.О. владельца не обнаружено");
	        }
        }
    }
}