
namespace X2J.Core.ReadXsd
{
    using System.Linq;
    using System.Xml.Schema;

    /// <summary>
    /// Reads the XmlSchemaAnnotationElement
    /// </summary>
    public static class ReadAnnotation
    {
        /// <summary>
        /// Returns the documentation present in the XmlSchemaAnnotation
        /// </summary>
        /// <param name="annotation">XmlSchemaAnnotation</param>
        /// <returns>Documentation attribute of the XmlSchemaAnnotation. Returns an Empty string if the annotation is null.</returns>
        public static string GetDocumentation(this XmlSchemaAnnotation annotation)
        {
            if (annotation == null || annotation.Items == null) return string.Empty;
            var documentation = annotation.Items.OfType<XmlSchemaDocumentation>()
                                          .Aggregate(" ", (current, item) => current + (item.Markup[0].InnerText + " ,"));
            return documentation.Substring(0, documentation.Length - 1).TrimStart().TrimEnd().Replace("\n",string.Empty).Replace("\r",string.Empty).Replace("\t",string.Empty);
         }
    }
}