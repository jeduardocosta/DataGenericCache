using DataGenericCache.Settings;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Settings
{
    [TestFixture]
    public class ServerSettingsTest
    {
        [Test]
        public void Equals_GivenTwoServerSettingsObjects_TheyMustBeEquals()
        {
            var serverSettingsA = new ServerSettings("address", 1234, "password");
            var serverSettingsB = new ServerSettings("address", 1234, "password");

            serverSettingsA.Should().Be(serverSettingsB);
        }
    }
}