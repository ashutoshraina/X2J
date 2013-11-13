namespace X2J.Core.ReadXsd
{
    using System.Collections.Generic;
    using System.Xml.Schema;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Schema;
    using Util;

    /// <summary>
    /// Reads the XmlSchemaAttribute
    /// </summary>
    public static class ReadAttribute
    {
        /// <summary>
        /// Extension method, returns JsonSchema equivalent of the XmlSchemaAttribute
        /// </summary>
        /// <param name="attribute">XmlSchemAttribute</param>
        /// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
        /// <returns></returns>
        public static JsonSchema ProcessAttribute(this XmlSchemaAttribute attribute, Formatting formatting)
        {
            var schema = new JsonSchema();
            if (attribute.Use == XmlSchemaUse.Prohibited) return schema;
            if (attribute.AttributeSchemaType != null)
            {
                string format;
                schema.Type = attribute.GetSchemaType(out format);
                if (format != null)
                    schema.Format = format;

                if (attribute.AttributeSchemaType.Content != null)
                {
                    if (attribute.AttributeSchemaType.Content is XmlSchemaSimpleTypeRestriction)
                    {
                        schema.AddRestrictions(attribute.AttributeSchemaType.Content as XmlSchemaSimpleTypeRestriction,
                                               formatting);
                    }
                    else if (attribute.AttributeSchemaType.Content is XmlSchemaSimpleTypeList)
                    {
                        var list = attribute.AttributeSchemaType.Content as XmlSchemaSimpleTypeList;
                        schema.Type = JsonSchemaType.Array;
                        var itemschema = new JsonSchema {Type = (JsonSchemaType) list.ItemTypeName.Name.GetTypeCode()};
                        schema.Items = new List<JsonSchema> {itemschema};
                    }
                }
            }
            if (!string.IsNullOrEmpty(attribute.Annotation.GetDocumentation()))
                schema.Description = attribute.Annotation.GetDocumentation();
            if (attribute.DefaultValue != null)
                schema.Default = attribute.DefaultValue;
            if (attribute.Use == XmlSchemaUse.Required)
                schema.Required = true;
            return schema;
        }
    }
}