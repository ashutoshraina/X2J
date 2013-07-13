namespace X2J.Core.Tests
{
    using System.Linq;
    using System.Xml;
    using System.Xml.Schema;
    using NUnit.Framework;
    using ReadXsd;

    public class ReadXmlSchemaTests
    {
        [Test]
        public void Annotaion_Should_Return_Documentation_And_AppInfo()
        {
            var reader = new XmlTextReader(@"XSD/Annotation.xsd");
            var schema = XmlSchema.Read(reader, null);

            var element = schema.Items.OfType<XmlSchemaElement>().FirstOrDefault();
            var result = element.Annotation.GetDocumentation();
            Assert.AreEqual("Sample Documentation , Sample Annotation", result);
        } 
    }
}