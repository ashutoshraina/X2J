namespace X2J.Core.Tests
{
	using System.Linq;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Schema;
	using NUnit.Framework;
	using PopulateJsonSchema;
	using Formatting = Newtonsoft.Json.Formatting;

	[TestFixture]
	public class GenerateSchema {
		public string PathToXsd { get; set; }

		public string PathToJsonSchema { get; set; }

		[TestFixtureSetUp]
		public void Initialise () {
			PathToJsonSchema = @"..\..\JsonSchema";
			PathToXsd = @"XSD";
		}

		[Test]
		public void Create_Json_Schema_From_XSD_OnDisk () {
			// Act - Sending in 6 schemas
			var jsonSchemas = WriteSchema.CreateJsonSchemaFromXsd(PathToXsd, PathToJsonSchema, Formatting.Indented).ToList();

			//Assert - We get six back
			Assert.AreEqual(7, jsonSchemas.Count);
		}

		[Test]
		public void Create_Json_Schema_From_XmlSchema () {
			//Arrange
			// Get the assembly that contains the embedded schema
			XmlSchema xmlSchema;
			const string schemaName = "Mails.xsd";
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("X2J.Core.Tests.XSD." + schemaName))
			using (var reader = XmlReader.Create(stream)) {
				xmlSchema = XmlSchema.Read(reader, ValidationEventHandler);
			}

			//Act
			var jsonSchema = WriteSchema.CreateJsonSchemaFromXmlSchema(schemaName, xmlSchema, PathToJsonSchema, Formatting.Indented);

			//Assert
			Assert.AreEqual("Mails", jsonSchema.Title);
			Assert.AreEqual(12, jsonSchema.Properties.Count);
		}

		[Test]
		public void Create_Json_Schema_From_XmlSchema_With_XmlSchemaGroup () {
			//Arrange
			// Get the assembly that contains the embedded schema
			XmlSchema xmlSchema;
			const string schemaName = "XMLSchemaGroupSample.xsd";
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream("X2J.Core.Tests.XSD." + schemaName))
			using (var reader = XmlReader.Create(stream)) {
				xmlSchema = XmlSchema.Read(reader, ValidationEventHandler);
			}

			//Act
			var jsonSchema = WriteSchema.CreateJsonSchemaFromXmlSchema(schemaName, xmlSchema, PathToJsonSchema, Formatting.Indented);

			//Assert
			Assert.AreEqual("XMLSchemaGroupSample", jsonSchema.Title);
			Assert.AreEqual(1, jsonSchema.Properties.Count);
		}

		private void ValidationEventHandler (object sender,ValidationEventArgs e) {
			//do nothing we are not interested in validation here
		}
	}
}