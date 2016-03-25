using DataGenericCache.Parsers;
using DataGenericCache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Parsers
{
    [TestFixture]
    public class JsonParserTest
    {
        private IJsonParser _jsonParser;

        private const string JsonContent = @"{""Address"":""address"",""Port"":0,""Password"":""pass""}";

        [SetUp]
        public void SetUp()
        {
            _jsonParser = new JsonParser();
        }

        [Test]
        public void ConvertObject_ToJson_UsingParser()
        {
            _jsonParser
                .Parse(GivenServerSettings())
                .Should()
                .Be(JsonContent);
        }

        [Test]
        public void ConvertJson_ToObject_UsingParser()
        {
            _jsonParser
                .Parse<ServerSettings>(JsonContent)
                .Should()
                .Be(GivenServerSettings());
        }

        [Test]
        public void Parse_GivenNullValue_ShouldReturnDefaultTypeValue()
        {
            _jsonParser
                .Parse<string>(null)
                .Should()
                .Be(default(string));
        }

        private static ServerSettings GivenServerSettings()
        {
            return new ServerSettings("address", 0, "pass");
        }
    }
}