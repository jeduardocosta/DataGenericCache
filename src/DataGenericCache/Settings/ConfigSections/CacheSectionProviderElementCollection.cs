using System;
using System.Configuration;

namespace DataGenericCache.Settings.ConfigSections
{
    [ConfigurationCollection(typeof(CacheSectionProviderElement))]
    internal class CacheSectionProviderElementCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "provider";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new CacheSectionProviderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CacheSectionProviderElement)element).Id;
        }

        public ConfigurationElement this[int idx]
        {
            get
            {
                return BaseGet(idx);
            }
        }
    }
}