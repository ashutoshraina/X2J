namespace X2J.Core.PopulateJsonSchema
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Xml;
	using System.Xml.Schema;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Newtonsoft.Json.Schema;
	using ReadXsd;
	using Util;
	using Formatting = Newtonsoft.Json.Formatting;

	/// <summary>
	/// Writes the JsonSchema
	/// </summary>
	public static class WriteSchema {
		/// <summary>
		/// Gets the TargetSchema of the XML document.
		/// </summary>
		/// <param name="filepath">Path to the XML document</param>
		/// <returns></returns>
		private static string GetTargetSchema (string filepath) {
			var doc = new XmlDocument();
			doc.Load(filepath);
			string attributeValue;
			return doc.OfType<XmlElement>().First().TryGetValue("targetNamespace", out attributeValue) ? attributeValue : "";
		}

		/// <summary>
		/// Wrties the Validation errors to the console
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="args">ValidationEventArgs</param>
		private static void ValidationCallback (object sender,System.Xml.Schema.ValidationEventArgs args) {
			switch (args.Severity) {
				case XmlSeverityType.Warning:
					Console.Write("WARNING: ");
					break;
				case XmlSeverityType.Error:
					Console.Write("ERROR: ");
					break;
			}
			Console.WriteLine(args.Message);
		}

		/// <summary>
		/// Writes JsonSchema to the File
		/// </summary>
		/// <param name="filePath">Full path of the file</param>
		/// <param name="jsonSchema">JsonSchema to write</param>
		/// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
		public static void WriteJson (string filePath,JsonSchema jsonSchema,Formatting formatting = Formatting.None) {
			var json = JsonConvert.SerializeObject(jsonSchema, formatting,
				           new JsonSerializerSettings {
					NullValueHandling = NullValueHandling.Ignore,
					Converters = new BindingList<JsonConverter> { new StringEnumConverter() }
				}
			           );
			File.WriteAllText(string.Format("{0}.js", filePath), json);
		}

		/// <summary>
		/// Creates JsonSchema from XSD
		/// </summary>
		/// <param name="xsdDirectory">Directory where XSD files are located</param>
		/// <param name="jsonSchemaDirectory">Directory for JsonSchema</param>
		/// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
		public static IEnumerable<JsonSchema> CreateJsonSchemaFromXsd (string pathToXsdDirectory,string pathToJsonSchemaDirectory,Formatting formatting = Formatting.None) {
			if ( pathToXsdDirectory == null )
				throw new ArgumentNullException("pathToXsdDirectory");
			if ( pathToJsonSchemaDirectory == null )
				throw new ArgumentNullException("pathToJsonSchemaDirectory");

			var files = Utility.NavigateDirectories(pathToXsdDirectory);
			var directoryInfo = new DirectoryInfo(pathToJsonSchemaDirectory);
			foreach (var file in files) {
				var jsonSchemadirectory = Directory.CreateDirectory(directoryInfo + "\\" + file.Name.Substring(0, file.Name.Length - 4));
				yield return CreateJsonSchema(file.Name, file.FullName, jsonSchemadirectory.FullName, formatting);
			}
		}

		/// <summary>
		/// Creates JsonSchema from string contents of an XSD
		/// </summary>
		/// <param name="xsdFileName">Name of the XSD or File</param>
		/// <param name="xsdContent">Content of the XSD</param>
		/// <param name="jsonSchemaDirectory">Directory to write the json schema </param>
		/// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
		public static JsonSchema CreateJsonSchemaFromXsdContent (string xsdFileName,string xsdContent,string jsonSchemaDirectory,Formatting formatting = Formatting.None) {
			var schemaReader = new StringReader(xsdContent);
			XmlSchema xmlSchema = XmlSchema.Read(schemaReader, (sender,args) => {
				ValidationCallback(sender, args);
			});
			return CreateJsonSchemaFromXmlSchema(xsdFileName, xmlSchema, jsonSchemaDirectory, formatting);
		}

		/// <summary>
		/// Created JsonSchema from XmlSchema
		/// </summary>
		/// <param name="xsdFileName">Name of the XmlSchema</param>
		/// <param name="xmlSchema">XmlSchema</param>
		/// <param name="jsonSchemaDirectory">Output directory for the JsonSchema</param>
		/// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
		/// <returns></returns>
		public static JsonSchema CreateJsonSchemaFromXmlSchema (string xsdFileName,XmlSchema xmlSchema,string jsonSchemaDirectory,Formatting formatting = Formatting.None) {
			var targetNamespace = xmlSchema.TargetNamespace;
			var schemaSet = new XmlSchemaSet();
			schemaSet.ValidationEventHandler += ValidationCallback;
			schemaSet.Add(xmlSchema);
			schemaSet.Compile();
			var baseschema = schemaSet.Schemas(targetNamespace).Cast<XmlSchema>().First();
			return CreateJsonSchema(xsdFileName, jsonSchemaDirectory, formatting, targetNamespace, schemaSet, baseschema);
		}

		/// <summary>
		/// Loads the file from the directory with the specified perimeters
		/// </summary>
		/// <param name="filename">FileName of the XSD document</param>
		/// <param name="filepath">Path to the File</param>
		/// <param name="directory"></param>
		/// <param name="formatting">Formatting for the JsonSchema. Should be None for production use</param>
		private static JsonSchema CreateJsonSchema (string xsdFileName,string xsdFilePath,string jsonSchemaDirectory,Formatting formatting) {
			var targetNamespace = GetTargetSchema(xsdFilePath);
			var schemaSet = new XmlSchemaSet();
			schemaSet.ValidationEventHandler += ValidationCallback;
			schemaSet.Add(targetNamespace, xsdFilePath);
			schemaSet.Compile();
			var baseschema = schemaSet.Schemas(targetNamespace).Cast<XmlSchema>().First();
			return CreateJsonSchema(xsdFileName, jsonSchemaDirectory, formatting, targetNamespace, schemaSet, baseschema);

		}

		/// <summary>
		/// Creates and writes the JSON Schema in the directory specified
		/// </summary>
		/// <param name="xsdFileName">Name of the XSD file</param>
		/// <param name="jsonSchemaDirectory">Directory for writing the JSON Schema</param>
		/// <param name="formatting">Formatting for the JsonSchema. Should be None for production use</param>
		/// <param name="targetNamespace">Targetnamespace</param>
		/// <param name="schemaSet">XmlSchemaSet</param>
		/// <param name="baseschema">BaseSchema of the XSD</param>
		private static JsonSchema CreateJsonSchema (string xsdFileName,string jsonSchemaDirectory,Formatting formatting,string targetNamespace,XmlSchemaSet schemaSet,XmlSchema baseschema) {
			var jsonschema = new JsonSchema {
				Title = xsdFileName.Substring(0, xsdFileName.Length - 4),
				Id = baseschema.TargetNamespace.StripXsdExtension(),
				Properties = new Dictionary<String, JsonSchema>()
			};

			foreach (var xmlSchemaAnnotation in
                                from annotation in schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                                                            .SelectMany(s => s.Items.OfType<XmlSchemaAnnotation>())
                                let description = annotation.GetDocumentation()
                                where !(string.IsNullOrEmpty(description))
                                select annotation) {
				jsonschema.Description = xmlSchemaAnnotation.GetDocumentation();
			}

			//get all the attributes
			foreach (var attr in from XmlSchema schema in schemaSet.Schemas(targetNamespace)
                                 let temp = schema.Attributes
                                 let schema1 = schema
                                 from attr in temp.Names.Cast<XmlQualifiedName>()
                                                        .Select(name => name)
                                                        .Select(t => schema1.Attributes[t]
                                                         as XmlSchemaAttribute)
                                 select attr) {
				var schema = attr.ProcessAttribute(formatting);
				schema.WriteSchemaToDirectory(jsonSchemaDirectory);
				jsonschema.Properties.Add(jsonschema.Properties.ContainsKey(attr.QualifiedName.Name)
                        ? attr.QualifiedName.Name.ToUpper()
                        : attr.QualifiedName.Name, schema);
			}

			//get all the attributeGroups
			foreach (var attrgroup in
                schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                         .SelectMany(s => s.Items.OfType<XmlSchemaAttributeGroup>())) {
				var schema = attrgroup.ProcessAttributeGroup(formatting);
				schema.WriteSchemaToDirectory(jsonSchemaDirectory);
				jsonschema.Properties.Add(attrgroup.QualifiedName.Name, schema);
			}

			//get all the Simple Types
			foreach (var simpletype in
                schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                         .SelectMany(s => s.Items.OfType<XmlSchemaSimpleType>())) {
				var schema = simpletype.ProcessSimpleType(formatting);
				schema.WriteSchemaToDirectory(jsonSchemaDirectory);
				jsonschema.Properties.Add(simpletype.QualifiedName.Name, schema);
			}

			//get all the ComplexTypes
			foreach (var complextype in schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                                                 .First().Items.OfType<XmlSchemaComplexType>()) {
				var schema = complextype.ProcessComplexType(formatting, jsonSchemaDirectory);
				jsonschema.Properties.Add(complextype.Name, schema);
			}

			//get all the elements
			foreach (var element in schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                                               .First().Items.OfType<XmlSchemaElement>()) {
				var schema = element.ProcessElement(formatting);
				schema.WriteSchemaToDirectory(jsonSchemaDirectory);
				jsonschema.Properties.Add(element.Name, schema);
			}

			//get all the groups
			foreach (var group in schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                                               .First().Items.OfType<XmlSchemaGroup>()) {
				var schema = group.ProcessGroup(formatting);
				schema.WriteSchemaToDirectory(jsonSchemaDirectory);
				jsonschema.Properties.Add(group.Name, schema);
			}

			return jsonschema;
		}
	}
}