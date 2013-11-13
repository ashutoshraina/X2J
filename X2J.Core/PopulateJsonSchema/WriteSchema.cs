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
    using Formatting = Newtonsoft.Json.Formatting;
    using ReadXsd;
    using Util;

    /// <summary>
    /// Writes the JsonSchema
    /// </summary>
    public static class WriteSchema
    {
        /// <summary>
        /// Gets the TargetSchema of the XML document.
        /// </summary>
        /// <param name="filepath">Path to the XML document</param>
        /// <returns></returns>
        private static string GetTargetSchema(string filepath)
        {
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
        private static void ValidationCallback(object sender, System.Xml.Schema.ValidationEventArgs args)
        {
            switch (args.Severity)
            {
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
        /// <param name="filePath">full path of the file</param>
        /// <param name="jsonSchema">JsonSchema to write</param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
        private static void WriteJson(string filePath, JsonSchema jsonSchema, Formatting formatting)
        {
            var json = JsonConvert.SerializeObject(jsonSchema, formatting, new JsonSerializerSettings
                                                                    {
                                                                        NullValueHandling = NullValueHandling.Ignore,
                                                                        Converters = new BindingList<JsonConverter> {new StringEnumConverter()}
                                                                    }
                                                );
            File.WriteAllText(filePath + ".js", json);
        }

        /// <summary>
        /// Creates JsonSchema from XSD
        /// </summary>
        /// <param name="xsdDirectory">Directory where XSD files are located</param>
        /// <param name="jsonSchemaDirectory">Directory for JsonSchema</param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
        public static void CreateJsonSchemaFromXsd(string xsdDirectory, string jsonSchemaDirectory, Formatting formatting = Formatting.Indented)
        {
            if (xsdDirectory == null)
                throw new ArgumentNullException("xsdDirectory");
            if (jsonSchemaDirectory == null)
                throw new ArgumentNullException("jsonSchemaDirectory");

            var files = Utility.NavigateDirectories(xsdDirectory);
            var directoryInfo = new DirectoryInfo(jsonSchemaDirectory);
            foreach (var file in files)
            {
                var directory = Directory.CreateDirectory(directoryInfo + "\\" + file.Name.Substring(0, file.Name.Length - 4));
                Load(file.Name, file.FullName, directory.FullName, formatting);
            }
        }

        /// <summary>
        /// Loads the file from the directory with the specified perimeters
        /// </summary>
        /// <param name="filename">FileName of the XSD document</param>
        /// <param name="filepath">Path to the File</param>
        /// <param name="directory"></param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use</param>
        private static void Load(string filename, string filepath, string directory, Formatting formatting )
        {
            var targetNamespace = GetTargetSchema(filepath);

            var schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler += ValidationCallback;
            schemaSet.Add(targetNamespace, filepath);
            schemaSet.Compile();
            var baseschema = schemaSet.Schemas(targetNamespace).Cast<XmlSchema>().First();

            var jsonschema = new JsonSchema
                             {
                                 Title = filename.Substring(0, filename.Length - 4),
                                 Id = baseschema.TargetNamespace.StripXsdExtension(),
                                 Properties = new Dictionary<String, JsonSchema>()
                             };

            foreach (var xmlSchemaAnnotation in
                from annotation in schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                                            .SelectMany(s => s.Items.OfType<XmlSchemaAnnotation>())
                let description = annotation.GetDocumentation()
                where !(string.IsNullOrEmpty(description))
                select annotation)
            {
                jsonschema.Description = xmlSchemaAnnotation.GetDocumentation();
            }

            //get all the attributes
            foreach (var attr in from XmlSchema schema in schemaSet.Schemas(targetNamespace)
                                 let temp = schema.Attributes
                                 let schema1 = schema
                                 from attr in temp.Names.Cast<XmlQualifiedName>().Select(name => name)
                                                  .Select(t => schema1.Attributes[t] as XmlSchemaAttribute)
                                 select attr)
            {
                jsonschema.Properties.Add(
                    jsonschema.Properties.ContainsKey(attr.QualifiedName.Name)
                        ? attr.QualifiedName.Name.ToUpper()
                        : attr.QualifiedName.Name, attr.ProcessAttribute(formatting));
            }

            //get all the attributeGroups
            foreach (var attrgroup in
                schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                         .SelectMany(s => s.Items.OfType<XmlSchemaAttributeGroup>()))
            {
                jsonschema.Properties.Add(attrgroup.QualifiedName.Name, attrgroup.ProcessAttributeGroup(formatting));
            }

            //get all the Simple Types
            foreach (var simpletype in
                schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                         .SelectMany(s => s.Items.OfType<XmlSchemaSimpleType>()))
            {
                jsonschema.Properties.Add(simpletype.QualifiedName.Name, simpletype.ProcessSimpleType(formatting));
            }

            //get all the ComplexTypes
            foreach (var complextype in schemaSet.Schemas(targetNamespace).Cast<XmlSchema>()
                                                 .First().Items.OfType<XmlSchemaComplexType>())
            {
                jsonschema.Properties.Add(complextype.Name, complextype.ProcessComplexType(formatting, directory));
            }

            //get all the elements
            foreach (
                var element in
                    schemaSet.Schemas(targetNamespace).Cast<XmlSchema>().First().Items.OfType<XmlSchemaElement>())
            {
                jsonschema.Properties.Add(element.Name, element.ProcessElement(formatting));
            }

            WriteJson(directory, jsonschema, formatting);
        }
    
    }
}