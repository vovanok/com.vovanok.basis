using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityBasis.Common;

namespace UnityBasis.Ui
{
    public class ScrollList<TData, TItem> : ScrollRect
        where TItem : ScrollListItem<TData>
        where TData : IComparable<TData>
    {
        private IComponentCreator componentCreator;

        public readonly UnityEvent<TData> OnSelectionChanged = new();
        public TItem SelectedItem { get; private set; }

        private readonly List<TItem> items = new();
        private bool isNeedResort;
        private TItem itemForScroll;

        [Inject]
        public void Construct(IComponentCreator componentCreator)
        {
            this.componentCreator = componentCreator;
        }

        public void UpdateItem(TData itemData)
        {
            var item = GetItem(itemData);

            if (item != null)
            {
                item.Init(itemData);
                isNeedResort = true;
            }
        }

        public void SelectItem(TData itemData)
        {
            if (itemData == null)
                return;

            var item = GetItem(itemData);
            if (item == null || item == SelectedItem)
                return;

            if (SelectedItem != null)
                SelectedItem.SetSelection(false);

            item.SetSelection(true);
            SelectedItem = item;
        }

        public void ScrollTo(TData itemData)
        {
            var item = GetItem(itemData);
            if (item == null)
                return;

            ScrollTo(item.transform);
        }

        public void AddOrUpdateItem(TData itemData)
        {
            if (itemData == null)
                return;

            var item = GetItem(itemData);
            if (item == null)
            {
                item = componentCreator.Instantiate<TItem>();
                item.transform.SetParent(content, false);

                item.OnPress
                    .AddListener(itemData =>
                    {
                        if (itemData == null)
                            return;
                        
                        SelectItem(itemData);
                        OnSelectionChanged.Invoke(itemData);
                    });

                items.Add(item);
            }

            item.Init(itemData);

            isNeedResort = true;
        }

        public void RemoveItem(TData itemData)
        {
            var item = GetItem(itemData);
            if (item == null)
                return;

            items.Remove(item);
            componentCreator.Destroy(item);
        }

        private TItem GetItem(TData itemData)
        {
            return items.FirstOrDefault(button => ReferenceEquals(button.Data, itemData));
        }

        private void Update()
        {
            if (isNeedResort)
            {
                isNeedResort = false;

                int itemSiblingIndex = 0;
                foreach (var item in items.OrderBy(item => item.Data))
                {
                    item.transform.SetSiblingIndex(itemSiblingIndex++);
                }

                if (SelectedItem != null)
                    ScrollTo(SelectedItem.transform);
            }

            if (itemForScroll != null)
            {
                ScrollTo(itemForScroll.transform);
                itemForScroll = null;
            }
        }

        private void ScrollTo(Transform target)
        {
            Canvas.ForceUpdateCanvases();

            int buttonIndexInParent = target.GetSiblingIndex();
            int parentChildsCount = content.transform.childCount;

            float buttonNormalizedPosition = Mathf.Clamp01(1 - (float)buttonIndexInParent / (float)(parentChildsCount - 1));
            verticalNormalizedPosition = buttonNormalizedPosition;
        }
    }
}