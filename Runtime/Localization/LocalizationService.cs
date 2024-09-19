using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityBasis.Localization
{
    public class LocalizationService
    {
        private readonly ILocalizationStorage localizationStorage;

        private Dictionary<string, string> localizationMap = new();

        public string CurrentLanguageTag { get; private set; }
        public readonly UnityEvent OnLanguageChanged = new();

        public LocalizationService(ILocalizationStorage localizationStorage)
        {
            this.localizationStorage = localizationStorage;
            SetLanguage(localizationStorage.GetLanguage());
        }

        public string[] GetLanguagesTags()
        {
            return localizationStorage.GetAvailableLanguagesTags();
        }

        public void SetLanguage(string languageKey)
        {
            if (CurrentLanguageTag == languageKey)
                return;
            
            CurrentLanguageTag = languageKey;
            localizationMap = localizationStorage.GetLocalizationMap(languageKey);
            localizationStorage.SetLanguage(languageKey);
            OnLanguageChanged.Invoke();
        }

        public string GetText(string localizationKey)
        {
            return localizationMap.GetValueOrDefault(localizationKey, localizationKey);
        }
    }
}