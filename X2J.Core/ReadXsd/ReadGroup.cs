
namespace X2J.Core.ReadXsd
{
    using Newtonsoft.Json.Schema;
    using System;
    using System.Collections.Generic;
    using System.Xml.Schema;
    using Formatting = Newtonsoft.Json.Formatting;
    using System.Linq;
    using Newtonsoft.Json.Linq;
    using Util;

    /// <summary>
    /// Reads XmlSchemaGroup
    /// </summary>
    public static class ReadGroup
    {
        /// <summary>
        /// Extension Method converts the XmlSchemaGroup to JsonSchema
        /// </summary>
        /// <param name="group">XmlSchemaGroup</param>
        /// <param name="formatting">Formatting for the schema</param>
        /// <returns>JsonSchema representation of the XmlSchemaGroup</returns>
        public static JsonSchema ProcessGroup(this XmlSchemaGroup group, Formatting formatting)
        {
            var schema = new JsonSchema { Id = string.Format("/{0}#",group.Name), Title = group.Name, Properties = new Dictionary<String, JsonSchema>() };
            var description = group.Annotation.GetDocumentation();
            if (!string.IsNullOrEmpty(description))
                schema.Description = description;
            var list = new List<String>();

            if (group.Particle is XmlSchemaSequence)
            {
                foreach (var item in group.Particle.Items)
                {
                    var element = item as XmlSchemaElement;
                    schema.Properties.Add(element.QualifiedName.Name, element.ProcessElement(formatting));
                }
            }
            else if (group.Particle is XmlSchemaChoice)
            {
                foreach (var item in group.Particle.Items)
                {
                  list.AddRange((item as XmlSchemaChoice).ProcessXmlSchemaChoice());
                }
            }
            else
	        {
                foreach (var item in group.Particle.Items)
                {
                    var xmlSchemaElement = item as XmlSchemaElement;
                    if (xmlSchemaElement != null)
                    {
                        schema.Properties.Add(xmlSchemaElement.QualifiedName.Name, xmlSchemaElement.ProcessElement(formatting));
                    }
                }
	        }
            
            schema.Enum = JToken.Parse(list.ToJsonString(formatting)).ToList();
            return schema;
        }
    }
}
