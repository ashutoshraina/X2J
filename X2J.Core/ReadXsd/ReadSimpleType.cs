namespace X2J.Core.ReadXsd
{
    using System.Collections.Generic;
    using System.Xml.Schema;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Schema;

    /// <summary>
    /// Reads the XmlSchemaSimpleType
    /// </summary>
    public static class ReadSimpleType
    {
        /// <summary>
        /// Extension Method, returns the JsonSchema equivalent of the XmlSchemaSimpleType
        /// </summary>
        /// <param name="simpleType">XmlSchemaType</param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
        /// <returns></returns>
        public static JsonSchema ProcessSimpleType(this XmlSchemaSimpleType simpleType, Formatting formatting)
        {
            var schema = new JsonSchema {Type = ((JsonSchemaType) simpleType.BaseXmlSchemaType.TypeCode)};
            var description = simpleType.Annotation.GetDocumentation();
            if (!string.IsNullOrEmpty(description))
                schema.Description = description;
            if (simpleType.Content != null)
            {
                if (simpleType.Content is XmlSchemaSimpleTypeRestriction)
                    schema.AddRestrictions(simpleType.Content, formatting);
                else if (simpleType.Content is XmlSchemaSimpleTypeList)
                {
                    schema.Type = JsonSchemaType.Array;
                    var itemschema = new JsonSchema
                                     {
                                         Type = (JsonSchemaType) (simpleType.Content as XmlSchemaSimpleTypeList).ItemTypeName.Name.GetTypeCode()
                                     };
                    schema.Items = new List<JsonSchema> {itemschema};
                }
                else if (simpleType.Content is XmlSchemaSimpleTypeUnion)
                {
                    var unionlist = (simpleType.Content as XmlSchemaSimpleTypeUnion).BaseMemberTypes;
                    foreach (var u in unionlist)
                        schema.AddRestrictions(u.Content, formatting);
                }
            }
            return schema;
        }
    }
}