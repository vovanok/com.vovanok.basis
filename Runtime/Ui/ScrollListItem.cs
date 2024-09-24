using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UnityBasis.Ui
{
    public abstract class ScrollListItem<TData> : MonoBehaviour
    {
        [SerializeField] private Sprite selectionBackground;
        [SerializeField] private Color selectionBackgroundColor;
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        public TData Data { get; private set; }
        public UnityEvent<TData> OnPress { get; } = new();

        private Sprite originalBackground;
        private Color originalBackgroundColor;

        public bool IsSelected { get; private set; }

        private void Awake()
        {
            button.onClick.AddListener(ButtonClickHandle);
            originalBackground = image.sprite;
            originalBackgroundColor = image.color;
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(ButtonClickHandle);
        }

        public void Init(TData data)
        {
            Data = data;
            InternalInit();
        }

        public virtual void SetSelection(bool isSelected)
        {
            IsSelected = isSelected;
            image.sprite = isSelected ? selectionBackground : originalBackground;
            image.color = isSelected ? selectionBackgroundColor : originalBackgroundColor;
        }

        protected abstract void InternalInit();

        private void ButtonClickHandle()
        {
            OnPress.Invoke(Data);
        }
    }
}
