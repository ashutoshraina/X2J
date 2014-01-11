namespace X2J.Core.ReadXsd
{
	using Newtonsoft.Json;
	using Newtonsoft.Json.Schema;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Schema;

	/// <summary>
	/// Reads the XmlSchemaAttributeGroup
	/// </summary>
	public static class ReadAttributeGroup {
		/// <summary>
		/// Extension Method, returns the JsonSchema equivalent of the XmlSchemaAttributeGroup
		/// </summary>
		/// <param name="attributeGroup">XmlsSchemaAttributeGroup</param>
		/// <param name="formatting">Formatting for the JsonSchema. Should be None for production use.</param>
		/// <returns></returns>
		public static JsonSchema ProcessAttributeGroup (this XmlSchemaAttributeGroup attributeGroup,Formatting formatting) {
			var schema = new JsonSchema {
				Description = attributeGroup.Annotation.GetDocumentation(),
				Properties = new Dictionary<String, JsonSchema>(),
				Title = attributeGroup.Name,
				Id = string.Format("/{0}#", attributeGroup.Name)
			};
			foreach (var attribute in attributeGroup.Attributes.OfType<XmlSchemaAttribute>()) {
				var attributeschema = attribute.ProcessAttribute(formatting);
				schema.Properties.Add(attribute.QualifiedName.Name, attributeschema);
			}
			return schema;
		}
	}
}