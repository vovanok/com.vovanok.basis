using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

namespace UnityBasis.Ui
{
    public abstract class DataDropdown<TData> : TMP_Dropdown where TData : class
    {
        public readonly UnityEvent<TData> OnSelectionChanged = new();
        
        private readonly List<OptionWithData<TData>> dataOptions = new();
        private bool isSelectItemLocked;

        protected override void Awake()
        {
            base.Awake();

            onValueChanged.AddListener(HandleValueChanged);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            onValueChanged.RemoveListener(HandleValueChanged);
        }

        public void AddOrUpdateItem(TData data)
        {
            if (data == null)
                return;

            OptionWithData<TData> option = GetOption(data);
            if (option != null)
            {
                option.RefreshOption();
                RefreshShownValue();
                return;
            }

            option = new OptionWithData<TData>(data, GetName, new OptionData());
            option.RefreshOption();
            dataOptions.Add(option);

            AddOptions(new List<OptionData> { option.DropdownOption });
        }
        
        public void RemoveItem(TData data)
        {
            OptionWithData<TData> option = GetOption(data);
            if (option == null)
                return;

            dataOptions.Remove(option);

            ClearOptions();
            AddOptions(dataOptions.Select(item => item.DropdownOption).ToList());
        }

        public void SelectItem(TData data)
        {
            OptionWithData<TData> option = GetOption(data);
            if (option == null)
                return;

            isSelectItemLocked = true;
            value = dataOptions.IndexOf(option);
            isSelectItemLocked = false;
        }

        private void HandleValueChanged(int itemIndex)
        {
            if (isSelectItemLocked)
                return;

            OnSelectionChanged.Invoke(GetItemByIndex(itemIndex));
        }
        
        private TData GetItemByIndex(int index)
        {
            if (index < 0 || index >= dataOptions.Count)
                return null;

            return dataOptions.ElementAt(index).Data;
        }
        
        private OptionWithData<TData> GetOption(TData data)
        {
            return dataOptions.FirstOrDefault(item => IsDataEquals(item.Data, data));
        }
        
        protected abstract string GetName(TData data);

        protected virtual bool IsDataEquals(TData data0, TData data1)
        {
            return data0 == data1;
        }
    }
}
