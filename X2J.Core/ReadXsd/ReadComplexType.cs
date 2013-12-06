namespace X2J.Core.ReadXsd
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Schema;
    using Util;

    /// <summary>
    /// Reads the XmlSchemaComplexType
    /// </summary>
    public static class ReadComplexType
    {
        /// <summary>
        /// Extension Method, adds the attribute uses <see cref="XmlSchemaComplexType.AttributeUses"/>to the JsonsSchema 
        /// </summary>
        /// <param name="complexType">XmlSchemaComplexType</param>
        /// <param name="schema">JsonSchema which needs to be augmented with the Attribute Uses</param>
        /// <param name="formatting">Formatting for the schema</param>
        public static void GetAttributes(this XmlSchemaComplexType complexType, JsonSchema schema, Formatting formatting)
        {
            if (complexType == null) return;            
            if (complexType.AttributeUses.Count > 0)
            {
                var enumerator = complexType.AttributeUses.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var xmlSchemaAttribute = enumerator.Value as XmlSchemaAttribute;
                    if (xmlSchemaAttribute != null)
                        schema.Properties.Add(xmlSchemaAttribute.QualifiedName.Name, xmlSchemaAttribute.ProcessAttribute(formatting));
                }
            }
            if (complexType.Attributes.Count > 0)
            {
                var enumerator = complexType.Attributes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var xmlSchemaAttribute = enumerator.Current as XmlSchemaAttribute;
                    if (xmlSchemaAttribute != null)
                    {
                        if (!schema.Properties.ContainsKey(xmlSchemaAttribute.QualifiedName.Name))
                        schema.Properties.Add(xmlSchemaAttribute.QualifiedName.Name, xmlSchemaAttribute.ProcessAttribute(formatting));
                    }
                }
            }
        }

        /// <summary>
        /// Extension Method, returns the JsonSchema equivalent of XmlSchemaType
        /// </summary>
        /// <param name="complexType">XmlSchemaType</param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
        /// <param name="directory">Directory in which to create to ComplexType.</param>
        /// <returns></returns>
        public static JsonSchema ProcessComplexType(this XmlSchemaComplexType complexType, Formatting formatting, string directory)
        {
            var schema = new JsonSchema { Title = complexType.Name, 
                                          Id = string.Format("/{0}#",complexType.Name), 
                                          Properties = new Dictionary<String, JsonSchema>() };
            if (complexType.Datatype != null)
            {
                string format;
                schema.Type = complexType.Datatype.GetSchemaType(out format);
                if (format != null)
                    schema.Format = format;
            }
            if (!string.IsNullOrEmpty(complexType.Annotation.GetDocumentation()))
                schema.Description = complexType.Annotation.GetDocumentation();
            //attributeuses for complextypes
            complexType.GetAttributes(schema, formatting);
            var sequence = complexType.ContentTypeParticle as XmlSchemaSequence;
            if (sequence != null)
            {
                var items = new List<JsonSchema>();
                foreach (XmlSchemaParticle childParticle in sequence.Items)
                {
                    var element = (childParticle as XmlSchemaElement);
                    if (element == null) continue;
                    var item = element.ProcessElement(formatting);
                    item.Title = element.QualifiedName.Name;
                    item.Id = string.Format("{0}/{1}#", element.QualifiedName.Namespace.StripXsdExtension(), element.QualifiedName.Name);
                    item.WriteSchemaToDirectory(directory);
                    var innerschema = new JsonSchema
                                      {
                                          Id = string.Format("{0}/{1}#", element.QualifiedName.Namespace.StripXsdExtension(), element.QualifiedName.Name)
                                      };
                    items.Add(new JsonSchema { Title = element.Name, Extends = new List<JsonSchema> { innerschema } });
                } //addextends
                schema.Items = items;
            }
            var choice = complexType.ContentTypeParticle as XmlSchemaChoice;
            if (choice != null)
                schema.Enum = JToken.Parse(choice.ProcessXmlSchemaChoice().ToJsonString(formatting)).ToList();
            var content = complexType.ContentModel as XmlSchemaSimpleContent;
            if (content != null)
            {
                content.ProcessXmlSchemaSimpleContent(schema, formatting);                
            }
            return schema;
        }
    }
}