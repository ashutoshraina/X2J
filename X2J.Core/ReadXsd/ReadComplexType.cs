namespace X2J.Core.ReadXsd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Schema;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using Util;

    /// <summary>
    /// Reads the XmlSchemaComplexType
    /// </summary>
    public static class ReadComplexType
    {
        /// <summary>
        /// Extension Method, returns the JsonSchema equivalent of XmlSchemaType
        /// </summary>
        /// <param name="complexType">XmlSchemaType</param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
        /// <param name="directory">Directory in which to create to ComplexType.</param>
        /// <returns></returns>
        public static JsonSchema ProcessComplexType(this XmlSchemaComplexType complexType, Formatting formatting, string directory)
        {
            var schema = new JsonSchema {Properties = new Dictionary<String, JsonSchema>()};
            if (complexType.BaseXmlSchemaType != null)
                schema.Type = (JsonSchemaType) complexType.BaseXmlSchemaType.TypeCode;
            if (complexType.Annotation.GetDocumentation() != "")
                schema.Description = complexType.Annotation.GetDocumentation();
            //attributeuses for complextypes
            if (complexType.AttributeUses.Count > 0)
            {
                var enumerator = complexType.AttributeUses.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var xmlSchemaAttribute = enumerator.Current as XmlSchemaAttribute;
                    if (xmlSchemaAttribute != null)
                        schema.Properties.Add(xmlSchemaAttribute.QualifiedName.Name, xmlSchemaAttribute.ProcessAttribute(formatting));
                }
            }
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
                    item.Id = element.QualifiedName.Namespace.StripXsdExtension() + "/" + element.QualifiedName.Name + "#";
                    item.WriteSchemaToDirectory(directory);
                    var innerschema = new JsonSchema
                                      {
                                          Id = element.QualifiedName.Namespace.StripXsdExtension() + "/" + element.QualifiedName.Name + "#"
                                      };
                    items.Add(new JsonSchema {Title = element.Name, Extends = new List<JsonSchema> {innerschema}});
                } //addextends
                schema.Items = items;
            }
            var choice = complexType.ContentTypeParticle as XmlSchemaChoice;
            if (choice != null)
                schema.Enum = JToken.Parse(choice.ProcessXmlSchemaChoice().ToJsonString(formatting)).ToList();
            var content = complexType.ContentModel as XmlSchemaSimpleContent;
            if (content != null)
            {
                var xmlSchemaSimpleContentExtension = content.Content as XmlSchemaSimpleContentExtension;
                if (xmlSchemaSimpleContentExtension != null)
                    schema.Properties.Add(xmlSchemaSimpleContentExtension.BaseTypeName.Name,
                                          xmlSchemaSimpleContentExtension.ProcessSimpleContentExtension(formatting));
                var xmlSchemaSimpleContentRestriction = content.Content as XmlSchemaSimpleContentRestriction;
                if (xmlSchemaSimpleContentRestriction != null)
                    schema.Properties.Add(xmlSchemaSimpleContentRestriction.BaseTypeName.Name,
                                          xmlSchemaSimpleContentRestriction.ProcessSimpleContentRestriction(formatting));
            }
            return schema;
        }
    }
}