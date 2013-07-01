namespace X2J.Core.ReadXsd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Schema;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Schema;
    using Util;
    using Formatting = Newtonsoft.Json.Formatting;

    /// <summary>
    /// Reads the XmlSchemaElement
    /// </summary>
    public static class ReadElement
    {
        /// <summary>
        /// Extenions Method, returns the JsonSchema equivalent of the XmlSchemaElement
        /// </summary>
        /// <param name="element">XmlSchemaElement</param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
        /// <returns></returns>
        public static JsonSchema ProcessElement(this XmlSchemaElement element, Formatting formatting)
        {
            var schema = IterateOverElement(element, formatting);
            schema.Default = element.DefaultValue;
            if (schema.Properties != null)
            {
                if (schema.Properties.ContainsKey("type"))
                    schema.Properties.Remove("type");
                if (schema.Properties.ContainsKey("description"))
                    schema.Properties.Remove("description");
            }
            if (!element.SubstitutionGroup.IsEmpty)
            {
                var innerschema = new JsonSchema
                                  {
                                      Id =
                                          element.SubstitutionGroup.Namespace + "/" +
                                          element.SubstitutionGroup.Name + "#"
                                  };
                schema.AdditionalProperties = new JsonSchema {Extends = new List<JsonSchema> {innerschema}};
            }
            if (element.MinOccursString != null && element.MinOccurs != 0)
                schema.MinimumItems = Convert.ToInt32(element.MinOccurs);
            if (element.MaxOccursString != null && !element.MaxOccursString.Equals("unbounded"))
                schema.MaximumItems = Convert.ToInt32(element.MaxOccurs);
            if (!element.Annotation.GetDocumentation().Equals(""))
                schema.Description = element.Annotation.GetDocumentation();
            if (element.ElementSchemaType != null) schema.Type = (JsonSchemaType) element.ElementSchemaType.TypeCode;

            return schema;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        private static JsonSchema IterateOverElement(XmlSchemaElement element, Formatting formatting)
        {
            var complexType = element.ElementSchemaType as XmlSchemaComplexType;

            if (complexType == null)
            {
                var simpleType = element.ElementSchemaType as XmlSchemaSimpleType;
                return simpleType.ProcessSimpleType(formatting);
            }
            return ProcessComplexTypeFromElement(complexType, formatting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="complexType"></param>
        /// <param name="formatting"></param>
        /// <returns></returns>
        private static JsonSchema ProcessComplexTypeFromElement(XmlSchemaComplexType complexType, Formatting formatting)
        {
            //only attributes of element
            var schema = new JsonSchema {Properties = new Dictionary<String, JsonSchema>()};
            if (complexType.Attributes.Count > 0)
            {
                var enumerator = complexType.Attributes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var xmlSchemaAttribute = enumerator.Current as XmlSchemaAttribute;
                    if (xmlSchemaAttribute != null)
                        schema.Properties.Add(xmlSchemaAttribute.QualifiedName.Name,
                                              xmlSchemaAttribute.ProcessAttribute(formatting));
                }
            }
            var sequence = complexType.ContentTypeParticle as XmlSchemaSequence;
            if (sequence != null)
            {
                foreach (var element in sequence.Items.Cast<XmlSchemaParticle>()
                                                .Select(childParticle => (childParticle as XmlSchemaElement))
                                                .Where(element => element != null))
                {
                    if (element == null) continue;
                    var item = new JsonSchema();
                    if (element.ElementSchemaType is XmlSchemaSimpleType)
                        item = (element.ElementSchemaType as XmlSchemaSimpleType).ProcessSimpleType(formatting);
                    else if (element.ElementSchemaType is XmlSchemaComplexType)
                    {
                        var innnercomplextype = element.ElementSchemaType as XmlSchemaComplexType;
                        var innerchoice = (innnercomplextype).ContentTypeParticle as XmlSchemaChoice;
                        if (innerchoice != null)
                            item.Enum =
                                JToken.Parse(innerchoice.ProcessXmlSchemaChoice().ToJsonString(Formatting.None))
                                      .ToList();
                    }
                    item.Title = element.QualifiedName.Name;
                    item.Id = element.QualifiedName.Namespace + "/" + element.QualifiedName.Name + "#";
                    item.Description = element.Annotation.GetDocumentation();

                    var innerschema = new JsonSchema {Id = item.Id};
                    schema.Properties.Add(schema.Properties.ContainsKey(item.Title) ? item.Title.ToUpper() : item.Title,
                                          new JsonSchema
                                          {
                                              Title = element.Name,
                                              Extends = new List<JsonSchema> {innerschema}
                                          });
                }
            }
            var choice = complexType.ContentTypeParticle as XmlSchemaChoice;
            if (choice != null)
                schema.Enum = JToken.Parse(choice.ProcessXmlSchemaChoice().ToJsonString(Formatting.None)).ToList();
            var content = complexType.ContentModel as XmlSchemaSimpleContent;
            if (content != null)
            {
                if (content.Content is XmlSchemaSimpleContentExtension)
                    schema.Properties.Add((content.Content as XmlSchemaSimpleContentExtension).BaseTypeName.Name,
                                          (content.Content as XmlSchemaSimpleContentExtension)
                                              .ProcessSimpleContentExtension(formatting));
                if (content.Content is XmlSchemaSimpleContentRestriction)
                    schema.Properties.Add((content.Content as XmlSchemaSimpleContentRestriction).BaseTypeName.Name,
                                          (content.Content as XmlSchemaSimpleContentRestriction)
                                              .ProcessSimpleContentRestriction(formatting));
            }
            return schema;
        }
    }
}