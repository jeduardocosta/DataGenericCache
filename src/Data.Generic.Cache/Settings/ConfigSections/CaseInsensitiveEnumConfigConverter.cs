using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace Data.Generic.Cache.Settings.ConfigSections
{
    public class CaseInsensitiveEnumConfigConverter<T> : ConfigurationConverterBase
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo cultureInfo, object data)
        {
            if (string.IsNullOrEmpty(data as string))
                throw new ArgumentException("failed to convert insensitive enum config. data parameter value is null or empty.");

            return Enum.Parse(typeof(T), (string)data, true);
        }
    }
}