using UnityEngine;
using Zenject;
using TMPro;

namespace UnityBasis.Localization
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        private LocalizationService localizationService;

        [SerializeField] private string localizationKey;

        private TMP_Text textComponent;
        
        [Inject]
        public void Construct(LocalizationService localizationService)
        {
            this.localizationService = localizationService;
        }
        
        private void Awake()
        {
            textComponent = GetComponent<TMP_Text>();

            RefreshText();
            localizationService.OnLanguageChanged
                .AddListener(RefreshText);
        }

        private void OnDestroy()
        {
            localizationService.OnLanguageChanged
                .RemoveListener(RefreshText);
        }

        private void RefreshText()
        {
            if (string.IsNullOrEmpty(localizationKey))
                return;
            
            textComponent.text = localizationService.GetText(localizationKey);
        }
    }
}
