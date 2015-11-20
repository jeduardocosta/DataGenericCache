using Data.Generic.Cache.Parsers;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Parsers
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
                .Parse(GivenAServerSettings())
                .Should()
                .Be(JsonContent);
        }

        [Test]
        public void ConvertJson_ToObject_UsingParser()
        {
            _jsonParser
                .Parse<ServerSettings>(JsonContent)
                .Should()
                .Be(GivenAServerSettings());
        }

        [Test]
        public void Parse_GivenNullValue_ShouldReturnDefaultTypeValue()
        {
            _jsonParser
                .Parse<string>(null)
                .Should()
                .Be(default(string));
        }

        private ServerSettings GivenAServerSettings()
        {
            return new ServerSettings("address", 0, "pass");
        }
    }
}