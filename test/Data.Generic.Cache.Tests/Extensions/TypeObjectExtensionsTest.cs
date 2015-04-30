using Data.Generic.Cache.Extensions;
using Data.Generic.Cache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Extensions
{
    [TestFixture]
    public class TypeObjectExtensionsTest
    {
        [Test]
        public void Should_ConvertAObject_ToJson()
        {
            new ServerSettings("server-value", 1234, "pass")
                .ToJson()
                .Should()
                .Be(@"{""Address"":""server-value"",""Port"":1234,""Password"":""pass""}");
        }
    }
}