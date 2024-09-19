using System.Collections.Generic;

namespace UnityBasis.Localization
{
    public interface ILocalizationStorage
    {
        string GetLanguage();
        void SetLanguage(string languageTag);
        Dictionary<string, string> GetLocalizationMap(string languageKey);
        string[] GetAvailableLanguagesTags();
    }
}
