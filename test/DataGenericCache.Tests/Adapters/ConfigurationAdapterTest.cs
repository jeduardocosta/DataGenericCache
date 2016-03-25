using DataGenericCache.Adapters;
using DataGenericCache.Settings.ConfigSections;
using FluentAssertions;
using NUnit.Framework;

namespace DataGenericCache.Tests.Adapters
{
    [TestFixture]
    public class ConfigurationAdapterTest
    {
        private IConfigurationAdapter _configurationAdapter;

        [SetUp]
        public void SetUp()
        {
            _configurationAdapter = new ConfigurationAdapter();
        }

        [Test]
        public void Should_GetValue_FromConfigurationFile()
        {
            const string sampleContent = "sample";

            _configurationAdapter
                .Get(sampleContent)
                .Should()
                .Be(sampleContent);
        }

        [Test]
        public void Should_ConvertDataGenericCacheSection_FromConfigurationFile()
        {
            _configurationAdapter
                .GetSections<object>("dataGenericCache")
                .Should()
                .NotBeNull();
        }

        [Test]
        public void Should_ConvertDataGenericCacheSection_AndCastToEntryType_FromConfigurationFile()
        {
            _configurationAdapter
                .GetSections<CacheSection>("dataGenericCache")
                .Should()
                .BeOfType<CacheSection>();
        }
    }
}