﻿using Data.Generic.Cache.Adapters;
using Data.Generic.Cache.Settings.ConfigSections;
using FluentAssertions;
using NUnit.Framework;

namespace Data.Generic.Cache.Tests.Adapters
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

            _configurationAdapter.Get(sampleContent)
                .Should()
                .Be(sampleContent);
        }

        [Test]
        public void Should_dataGenericCacheSection_FromConfigurationFile()
        {
            _configurationAdapter.GetSections<object>("dataGenericCacheSection")
                .Should()
                .NotBeNull();
        }

        [Test]
        public void Should_dataGenericCacheSection_AndCastToEntryType_FromConfigurationFile()
        {
            _configurationAdapter.GetSections<CacheSection>("dataGenericCacheSection")
                .Should()
                .BeOfType<CacheSection>();
        }
    }
}