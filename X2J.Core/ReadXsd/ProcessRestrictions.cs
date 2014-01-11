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

	public static class ProcessRestrictions {
		/// <summary>
		/// Adds the XmlSchemaRestriction to the JsonSchema
		/// </summary>
		/// <param name="schema">JsonSchema</param>
		/// <param name="formatting">Formatting for the schema. Should be None for production use</param>
		/// <param name="content">XmlSchemaTypeContent from which to extract the restrictions</param>
		public static void AddRestrictions (this JsonSchema schema,XmlSchemaSimpleTypeContent content,Formatting formatting) {
			if ( content == null )
				return;
			bool pattern;
			var result = (content as XmlSchemaSimpleTypeRestriction).ProcessSimpleTypeRestriction(out pattern);
			if ( pattern ) {
				var patternexpr = ((XmlSchemaSimpleTypeRestriction)content).Facets.OfType<XmlSchemaPatternFacet>().First().Value;
				schema.Pattern = patternexpr;
			} else if ( result != null )
				schema.Enum = result;
		}

		/// <summary>
		/// Gets a List of JToken which had the restrictions extracted out from the XmlSchemaTypeRestriction
		/// </summary>
		/// <param name="restriction">XmlSchemaTypeRestriction that needs to processed</param>
		/// <param name="pattern">bool flag indicating the existence of a pattern</param>
		/// <returns>A List of JToken with the restrictions or null if no restrictions are found</returns>
		public static List<JToken> ProcessSimpleTypeRestriction (this XmlSchemaSimpleTypeRestriction restriction,out bool pattern) {
			pattern = false;
			var res = new List<string>();
			res = restriction.Facets.OfType<XmlSchemaEnumerationFacet>().Select(t => t.Value).ToList();
			res.AddRange(restriction.Facets.OfType<XmlSchemaMaxExclusiveFacet>().Select(t => t.Value));
			res.AddRange(restriction.Facets.OfType<XmlSchemaMinExclusiveFacet>().Select(t => t.Value));
			if ( restriction.Facets.OfType<XmlSchemaPatternFacet>().Any() )
				pattern = true;
			return JToken.Parse(res.ToJsonString(Formatting.None)).ToList();
		}

		/// <summary>
		/// Gets the choices from the XmlSchemaChoice
		/// </summary>
		/// <param name="choice">XmlSchemaChoice</param>
		/// <returns>A List containing the choices</returns>
		public static List<String> ProcessXmlSchemaChoice (this XmlSchemaChoice choice) {
			var list = new List<String>();
			if ( choice == null )
				return list;            

			var schema = new JsonSchema { Properties = new Dictionary<String, JsonSchema>() };
            
			var description = choice.Annotation.GetDocumentation();            
			if ( !string.IsNullOrEmpty(description) )
				schema.Description = description;

			if ( choice.MinOccursString != null && choice.MinOccurs != 0 )
				schema.MinimumItems = Convert.ToInt32(choice.MinOccurs);
			if ( choice.MaxOccursString != null && !choice.MaxOccursString.Equals("unbounded") )
				schema.MaximumItems = Convert.ToInt32(choice.MaxOccurs);

			foreach (var item in choice.Items.Cast<XmlSchemaObject>().Where(item => item != null)) {
				var xmlSchemaElement = item as XmlSchemaElement;
				if ( xmlSchemaElement != null ) {
					list.Add(xmlSchemaElement.QualifiedName.Name);
				} else if ( item is XmlSchemaSequence ) {
					var temp = item as XmlSchemaSequence;
					list.AddRange(from XmlSchemaObject inneritem in temp.Items
					              select ((XmlSchemaElement)inneritem).QualifiedName.Name);
				} else if ( item is XmlSchemaChoice )
					list.AddRange((item as XmlSchemaChoice).ProcessXmlSchemaChoice());
			}
			return list;
		}

		/// <summary>
		/// Gets the JsonSchema equivalent of the XmlSchemaParticle
		/// </summary>
		/// <param name="childParticle">XmlSchemaParticle which needs to be converted</param>
		/// <returns>JsonSchema equivalent of the XmlSchemaParticle</returns>
		public static JsonSchema ProcessXmlSchemaParticle (this XmlSchemaParticle childParticle) {
			var schema = new JsonSchema();
			var element = (childParticle as XmlSchemaElement);
			if ( element == null )
				return null;
			if ( element.MinOccursString != null && element.MinOccurs != 0 )
				schema.MinimumItems = Convert.ToInt32(childParticle.MinOccurs);
			if ( childParticle.MaxOccursString != null && !childParticle.MaxOccursString.Equals("unbounded") )
				schema.MaximumItems = Convert.ToInt32(childParticle.MaxOccurs);
			if ( !string.IsNullOrEmpty(element.Annotation.GetDocumentation()) )
				schema.Description = element.Annotation.GetDocumentation();
			if ( element.DefaultValue != null )
				schema.Default = element.DefaultValue;
			return schema;
		}

		/// <summary>
		/// Gets the Content Restriction 
		/// </summary>
		/// <param name="formatting">Formatting for the schema. Should be None for production use.</param>
		/// <param name="contentRestriction">XmlSchemaContentRestriction</param>
		/// <returns>JsonSchema equivalent of the restriction</returns>
		public static JsonSchema ProcessSimpleContentRestriction (this XmlSchemaSimpleContentRestriction contentRestriction,Formatting formatting) {
			var schema = new JsonSchema {
				Type = (JsonSchemaType.Object),
				Properties = new Dictionary<String, JsonSchema>()
			};
			if ( contentRestriction.BaseType != null ) {
				string format;
				schema.Type = contentRestriction.BaseType.BaseXmlSchemaType.Datatype.GetSchemaType(out format);
				if ( format != null )
					schema.Format = format;
			}
			//add extends
			if ( contentRestriction.Attributes.Count > 0 ) {
				var enumerator = contentRestriction.Attributes.GetEnumerator();
				while (enumerator.MoveNext()) {
					var attribute = (XmlSchemaAttribute)enumerator.Current;
					if ( attribute == null )
						continue;
					schema.Properties.Add(attribute.QualifiedName.Name, attribute.ProcessAttribute(formatting));
				}
			}
			return schema;
		}

		/// <summary>
		/// Gets the JsonSchema equivalent of the 
		/// </summary>
		/// <param name="contentExtension">XmlSchemaSimpleContentExtension which needs to be processed</param>
		/// <param name="formatting">formatting for the JsonSchema. Should be None for production use</param>
		/// <returns>JsonSchema equivalent of the XmlSchemaSimpleContentExtension</returns>
		public static JsonSchema ProcessSimpleContentExtension (this XmlSchemaSimpleContentExtension contentExtension,Formatting formatting) {
			var schema = new JsonSchema {
				Type = (JsonSchemaType.Object),
				Properties = new Dictionary<String, JsonSchema>()
			};
			if ( contentExtension.Attributes.Count > 0 ) {
				var enumerator = contentExtension.Attributes.GetEnumerator();
				while (enumerator.MoveNext()) {
					var xmlSchemaAttribute = enumerator.Current as XmlSchemaAttribute;
					if ( xmlSchemaAttribute != null )
						schema.Properties.Add(xmlSchemaAttribute.QualifiedName.Name, xmlSchemaAttribute.ProcessAttribute(formatting));
					//for now removed attributegroupref
				}
			}
			return schema;
		}
	}
}