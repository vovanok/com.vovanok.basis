using System;
using TMPro;

namespace UnityBasis.Ui
{
    public class OptionWithData<T>
    {
        public readonly T Data;
        public readonly TMP_Dropdown.OptionData DropdownOption;

        private readonly Func<T, string> getName;

        public OptionWithData(T data, Func<T, string> getName, TMP_Dropdown.OptionData dropdownOption)
        {
            Data = data;
            DropdownOption = dropdownOption;
            this.getName = getName;
        }

        public void RefreshOption()
        {
            string name = getName(Data);
            DropdownOption.text = !string.IsNullOrEmpty(name)
                ? name
                : "<noname>";
        }
    }
}
