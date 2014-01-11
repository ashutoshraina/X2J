namespace X2J.Core.ReadXsd
{
	using System.Linq;
	using System.Xml.Schema;

	/// <summary>
	/// Reads the XmlSchemaAnnotationElement
	/// </summary>
	public static class ReadAnnotation {
		/// <summary>
		/// <para>
		/// Returns the documentation present in the XmlSchemaAnnotation. 
		/// Annotation can have (appinfo|documentation)* i.e zero or more appinfo or documentation.
		/// </para> 
		/// </summary>
		/// <param name="annotation">XmlSchemaAnnotation</param>
		/// <returns>Documentation attribute of the XmlSchemaAnnotation. Returns an Empty string if the annotation is null.</returns>
		public static string GetDocumentation (this XmlSchemaAnnotation annotation) {
			if ( annotation == null || annotation.Items == null )
				return string.Empty;
			var documentation = annotation.Items.OfType<XmlSchemaDocumentation>()
                                          .Aggregate(" ", (current,item) => current + (item.Markup[0].InnerText + " ,"));
			var appinfo = annotation.Items.OfType<XmlSchemaAppInfo>()
                                    .Aggregate(" ", (current,item) => current + (item.Markup[0].InnerText + ","));
			var constructedDocumentation = string.Concat(documentation, appinfo);
			return constructedDocumentation.Substring(0, constructedDocumentation.Length - 1).TrimStart().TrimEnd().Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
		}
	}
}