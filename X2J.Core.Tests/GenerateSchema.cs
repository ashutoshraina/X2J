namespace X2J.Core.Tests
{
    using NUnit.Framework;
    using PopulateJsonSchema;
    [TestFixture]
    public class GenerateSchema
    {
        public string PathToXsd { get; set; }
        public string PathToJsonSchema { get; set; }

        [TestFixtureSetUp]
        public void Initialise()
        {
            PathToJsonSchema = @"JsonSchema";
            PathToXsd = @"XSD";
        }

        [Test]
        public void Create_Json_Schema()
        {
            WriteSchema.CreateJsonSchemaFromXsd(PathToXsd,PathToJsonSchema);
        }
    }
}