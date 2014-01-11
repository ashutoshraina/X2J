namespace X2J.Core.Util
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Reflection;
	using System.Xml;
	using System.Xml.Schema;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Newtonsoft.Json.Schema;
	using Formatting = Newtonsoft.Json.Formatting;

	/// <summary>
	/// Contains the extension methods which are used in X2J
	/// </summary>
	internal static class Utility {
		/// <summary>
		/// Convets a enumerable of string into Json
		/// </summary>
		/// <param name="obj">Enumerable of String to be converted</param>
		/// <param name="formatting">Formatting of the Json output. Should be set to none for production use.</param>
		/// <returns>Json</returns>
		public static string ToJsonString (this IEnumerable<string> obj,Formatting formatting) {
			return JsonConvert.SerializeObject(obj, formatting);
		}

		/// <summary>
		/// Converts a JsonSchema to Json
		/// </summary>
		/// <param name="schema">JsonSchema to be converted</param>
		/// <param name="formatting">Formating for the Json output. Should be set to none for production use.</param>
		/// <returns>Json</returns>
		public static string ToJsonString (this JsonSchema schema,Formatting formatting) {
			return JsonConvert.SerializeObject(schema, formatting);
		}

		/// <summary>
		/// Replaces the .xsd with .js
		/// </summary>
		/// <param name="input">input string to strip the extension from</param>
		/// <returns>string with the replaced extension</returns>
		public static string StripXsdExtension (this string input) {
			if ( input == null || input.Length < 4 )
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
		public static bool TryGetValue (this XmlElement element,string attributeName,out string attributeValue) {
			var exists = element.HasAttribute(attributeName);
			attributeValue = default(string);
			if ( exists ) {
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
		public static T GetPrivateProperty<T> (object obj,string propertyName) where T: class {
			//null check introduced because mono was screaming about propInfo being null, never happened with .net on windows
			try {
				var propInfo = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
				if ( propInfo != null ) {
					return (T)propInfo.GetValue(obj, null);
				} else {
					return null;
				}

			} catch (Exception ex) {
				Console.WriteLine(string.Format("Error looking up private property {0} in {1}", propertyName, obj));
				Console.WriteLine(ex.Message);
				return null;
			}

		}

		/// <summary>
		/// Gets the corresponding JsonSchemaType for the XmlSchemaAttribute
		/// </summary>
		/// <param name="attribute">Attribute for which the JsonSchemaType is required</param>
		/// <param name="format">Format property ( currently only supports Date )</param>
		/// <returns>JsonSchemaType</returns>
		public static JsonSchemaType GetSchemaType (this XmlSchemaAttribute attribute,out string format) {
			// Access the internal property Datatype
			var dataType = GetPrivateProperty<XmlSchemaDatatype>(attribute, "Datatype");
			if ( dataType != null ) {
				var typeName = dataType.TypeCode.ToString();
				var result = ToJsonSchemaType(typeName);
				format = result.Item2;
				return result.Item1;
			}
			//we can't get to the datatype, let's make it as loose as possible
			format = "String";
			return JsonSchemaType.Any;
		}

		/// <summary>
		/// Converts XmlSchemaDatatype to JsonSchemaType
		/// </summary>
		/// <param name="xmlSchemaDatatype">XmlSchemaDatatype</param>
		/// <param name="format">out parameter which represents the format of the JsonSchemaType
		///     e.g. JsonSchemaType for both Date and Date-Time is string but the formats are 
		///         Date and Date-Time repsectively</param>
		/// <returns></returns>
		public static JsonSchemaType GetSchemaType (this XmlSchemaDatatype xmlSchemaDatatype,out string format) {
			var typeName = xmlSchemaDatatype.TypeCode.ToString();
			var result = ToJsonSchemaType(typeName);
			format = result.Item2;
			return result.Item1;
		}

		/// <summary>
		/// Returns a Tuple of JsonSchemaType,Format
		/// </summary>
		/// <param name="typeName">TypeName to convert from</param>
		/// <returns>Tuple containing the JsonSchemaType and the format represemted as string</returns>
		private static Tuple<JsonSchemaType, string> ToJsonSchemaType (string typeName) {
			JsonSchemaType jsonSchemaType = JsonSchemaType.Any;
			string format = "String";
            
			if ( typeName.Contains("Token", StringComparison.OrdinalIgnoreCase) || typeName.Contains("string", StringComparison.OrdinalIgnoreCase) ) {
				jsonSchemaType = JsonSchemaType.String;                
			} else if ( typeName.Contains("int", StringComparison.OrdinalIgnoreCase) ) {
				jsonSchemaType = JsonSchemaType.Integer;
				format = "Integer";            
			} else if ( typeName.Contains("Date", StringComparison.OrdinalIgnoreCase) ) {
				jsonSchemaType = JsonSchemaType.String;
				format = "Date";
				if ( typeName.Contains("Datetime", StringComparison.OrdinalIgnoreCase) ) {
					format = "Date-Time";
				}
			} else if ( typeName.Contains("Decimal", StringComparison.OrdinalIgnoreCase)
			            || (typeName.Contains("Float", StringComparison.OrdinalIgnoreCase))
			            || (typeName.Contains("Double", StringComparison.OrdinalIgnoreCase)) ) {
				jsonSchemaType = JsonSchemaType.Float;
				format = "Float";                
			} else if ( typeName.Contains("boolean", StringComparison.OrdinalIgnoreCase) ) {
				jsonSchemaType = JsonSchemaType.Boolean;                
			} else if ( typeName.Contains("base64", StringComparison.OrdinalIgnoreCase) ) {
				jsonSchemaType = JsonSchemaType.String;
				format = "byte";               
			}
			return new Tuple<JsonSchemaType, string>(jsonSchemaType, format);
		}

		/// <summary>
		/// Writes the JsonSchema to the specifed directory
		/// </summary>
		/// <param name="schema">Schema to be written</param>
		/// <param name="directory">Path of the directory</param>
		public static void WriteSchemaToDirectory (this JsonSchema schema,string directory) {
			var json = JsonConvert.SerializeObject(
				           schema,
				           Formatting.Indented,
				           new JsonSerializerSettings {	
					NullValueHandling = NullValueHandling.Ignore, 
					Converters = new BindingList<JsonConverter> { new StringEnumConverter() },
				}
			           );
			File.WriteAllText(directory + "\\" + schema.Title + ".js", json);
		}

		/// <summary>
		/// Navigates the directories with the given path and looks for XSD files.
		/// </summary>
		/// <param name="path">Path to navigate</param>
		/// <returns>An Enumerable of FileInfo</returns>
		public static IEnumerable<FileInfo> NavigateDirectories (String path) {
			return new DirectoryInfo(path).GetFiles("*.xsd", SearchOption.AllDirectories);
		}

		/// <summary>
		/// Checks whether the source string contains another string or not
		/// </summary>
		/// <param name="source">Source string to perform the operation on</param>
		/// <param name="toCheck">String to find</param>
		/// <param name="comp">StringComparison type</param>
		/// <returns></returns>
		private static bool Contains (this string source,string toCheck,StringComparison comp) {
			return source.IndexOf(toCheck, comp) >= 0;
		}
	}
}