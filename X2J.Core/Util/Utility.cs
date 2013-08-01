namespace X2J.Core.Util
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Schema;
    using Formatting = Newtonsoft.Json.Formatting;
    using System.Reflection;
    using System.Xml.Schema;
    using System.ComponentModel;
    using System.IO;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Contains the extension methods which are used in X2J
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Convets a enumerable of string into Json
        /// </summary>
        /// <param name="obj">Enumerable of String to be converted</param>
        /// <param name="formatting">Formatting of the Json output. Should be set to none for production use.</param>
        /// <returns>Json</returns>
        public static string ToJsonString(this IEnumerable<string> obj, Formatting formatting)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        /// <summary>
        /// Converts a JsonSchema to Json
        /// </summary>
        /// <param name="schema">JsonSchema to be converted</param>
        /// <param name="formatting">Formating for the Json output. Should be set to none for production use.</param>
        /// <returns>Json</returns>
        public static string ToJsonString(this JsonSchema schema, Formatting formatting)
        {
            return JsonConvert.SerializeObject(schema, formatting);
        }

        /// <summary>
        /// Replaces the .xsd with .js
        /// </summary>
        /// <param name="input">input string to strip the extension from</param>
        /// <returns>string with the replaced extension</returns>
        public static string StripXsdExtension(this string input)
        {
            if (input == null || input.Length < 4)
                return input;
            return input.Replace(".xsd", ".js");
        }

        /// <summary>
        /// Gets the value of an attribute from a XmlElement
        /// </summary>
        /// <param name="element">XmlElement</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="attributeValue">Value of the attribute</param>
        /// <returns>bool indicating the existence of the attribute</returns>
        public static bool TryGetValue(this XmlElement element, string attributeName, out string attributeValue)
        {
            var exists = element.HasAttribute(attributeName);
            attributeValue = default(string);
            if (exists)
            {
                attributeValue = element.Attributes[attributeName].Value;
            }
            return exists;
        }

        /// <summary>
        /// Gets the value of the private property for an object
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="obj">Object whose property needs to be accessed. Currently doesn't include static properties</param>
        /// <param name="propertyName">Property whose value is required</param>
        /// <returns>Type</returns>
        public static T GetPrivateProperty<T>(object obj, string propertyName)
        {
            return (T) obj.GetType()
                          .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic)
                          .GetValue(obj, null);
        }

        /// <summary>
        /// Gets the corresponding JsonSchemaType for the XmlSchemaAttribute
        /// </summary>
        /// <param name="attribute">Attribute for which the JsonSchemaType is required</param>
        /// <param name="format">Format property ( currently only supports Date )</param>
        /// <returns>JsonSchemaType</returns>
        public static JsonSchemaType GetSchemaType(this XmlSchemaAttribute attribute, out string format)
        {
            JsonSchemaType jsonSchemaType;
            format = null;
            // Access the internal property Datatype
            var dataType = GetPrivateProperty<XmlSchemaDatatype>(attribute, "Datatype");
            var typeName = dataType.TypeCode.ToString();

            if (typeName.Contains("NmToken"))
            {
                jsonSchemaType = JsonSchemaType.String;
            }
            else if (typeName.Contains("Date"))
            {
                jsonSchemaType = JsonSchemaType.String;
                format = "Date";
            }
            else
                jsonSchemaType = (JsonSchemaType) Enum.Parse(typeof (JsonSchemaType), typeName);
            return jsonSchemaType;
        }

        /// <summary>
        /// Writes the JsonSchema to the specifed directory
        /// </summary>
        /// <param name="schema">Schema to be written</param>
        /// <param name="directory">Path of the directory</param>
        public static void WriteSchemaToDirectory(this JsonSchema schema, string directory)
        {
            var json = JsonConvert.SerializeObject(
                schema,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new BindingList<JsonConverter> {new StringEnumConverter()},
                }
                );
            File.WriteAllText(directory + "\\" + schema.Title + ".js", json);
        }

        /// <summary>
        /// Navigates the directories with the given path and looks for XSD files.
        /// </summary>
        /// <param name="path">Path to navigate</param>
        /// <returns>An Enumerable of FileInfo</returns>
        public static IEnumerable<FileInfo> NavigateDirectories(String path)
        {
            return new DirectoryInfo(path).GetFiles("*.xsd", SearchOption.AllDirectories);
        }
    }
}