using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Keiho.Configuration
{
    public static class SettingsUtility
    {
        public static T GetAppSetting<T>(string key, T defaultValue)
        {
            string value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(value) ? defaultValue : value.To<T>();
        }

        public static T GetSectionSetting<T>(string sectionName, string key, T defaultValue)
        {
            object section = ConfigurationManager.GetSection(sectionName);

            if (section == null)
            {
                return defaultValue;
            }

            if (section is ConfigurationSection)
            {
                var configSection = (ConfigurationSection)section;

                throw new NotImplementedException();
            }

            if (section is Hashtable)
            {
                var hashtable = (Hashtable)section;

                // キーの大文字と小文字を区別しません。
                string strictKey = hashtable.Keys.Cast<string>()
                    .SingleOrDefault(k => string.Equals(k, key, StringComparison.InvariantCultureIgnoreCase));
                string value = strictKey == null ? null : (string)hashtable[strictKey];
                return string.IsNullOrEmpty(value) ? defaultValue : value.To<T>();
            }

            throw new InvalidOperationException();
        }
    }
}
