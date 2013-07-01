
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
        public void CreateJsonSchemaByProvidingPath()
        {
            WriteSchema.CreateJsonSchemaFromXsd(PathToXsd,PathToJsonSchema);
        }
    }
}