using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientTRICLib.Common;

namespace ClientTRICLib.tests.Common
{
    [TestClass]
    public class XmlBuilderTests
    {
        XmlBuilder builder, builder2;

        [TestMethod]
        public void Build_without_Attrs()
        {
            builder = new XmlBuilder("root");
            Assert.AreEqual("<root />", builder.Build().ToString());
        }

        [TestMethod]
        public void Build_with_Attrs()
        {
            string xml = @"<Options mode=""1"" isTest=""yes"" />";
            builder = new XmlBuilder("Options");
            builder.AddAttribute("mode", 1);
            builder.AddAttribute("isTest", "yes");

            Assert.AreEqual(xml, builder.Build().ToString());
        }

        [TestMethod]
        public void Build_with_Value()
        {
            string xml = @"<Root>value</Root>";
            builder = new XmlBuilder("Root");
            builder.AddValue("value");
            Assert.AreEqual(xml, builder.Build().ToString());
        }

        [TestMethod]
        public void Build_with_Empty()
        {
            builder = new XmlBuilder("");
            Assert.AreEqual("<root />", builder.Build().ToString());
        }

        [TestMethod]
        public void Build_with_isNull()
        {
            builder = new XmlBuilder(null);
            Assert.AreEqual("<root />", builder.Build().ToString());
        }

        [TestMethod]
        public void Build_with_AddElement()
        {
            string xml = @"<Accounts>
  <Account account=""1"" />
</Accounts>";
            builder = new XmlBuilder("Accounts");

            builder2 = new XmlBuilder("Account");
            builder2.AddAttribute("account", 1);

            builder.AddElement(builder2.Build());

            Assert.AreEqual(xml, builder.Build().ToString());
        }
    }
}
