using System.Collections.Generic;
using System.Xml.Linq;

namespace ClientTRICLib.Common
{
    public class XmlBuilder
    {
        XElement xElement;
        Dictionary<string, object> attributes;

        public XmlBuilder(string rootElementName)
        {
            rootElementName = (string.IsNullOrEmpty(rootElementName))
                ? "root" : rootElementName;
            xElement = new XElement(rootElementName);
            attributes = new Dictionary<string, object>();
        }

        public void AddAttribute(string attrName, object value)
        {
            attributes.Add(attrName, value);
        }

        public void AddElement(object element)
        {
            xElement.Add(element);
        }

        public void AddValue(object value)
        {
            this.xElement.SetValue(value);
        }

        public XElement Build()
        {
            foreach (var attr in attributes)
            {
                xElement.Add(new XAttribute(attr.Key, attr.Value));
            }
            return xElement;
        }
    }
}
