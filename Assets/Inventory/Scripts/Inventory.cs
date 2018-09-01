using System;
using System.Collections;
using System.Collections.Generic;
using uItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace uInventory
{
    public class Inventory<TTemplate, TInstance>
        where TTemplate : ItemTemplate, new()
    where TInstance : ItemInstance<TTemplate>, new()
    {
        public InventoryItemSlot<TTemplate, TInstance>[] ItemSlots { get; private set; }
        public int SlotCount { get; private set; }

        public delegate void ItemChangedDelegate(TInstance itemInstance);
        public event ItemChangedDelegate OnItemAdded = delegate { };

        private GameObject owner;
        private DataTemplateManager<TTemplate> dataTempateManager;

        public Inventory(GameObject inOwner, DataTemplateManager<TTemplate> templateManager, int slotCount)
        {
            this.owner = inOwner;
            this.SlotCount = slotCount;
            this.dataTempateManager = templateManager;

            // initialize item slots
            ItemSlots = new InventoryItemSlot<TTemplate, TInstance>[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                ItemSlots[i] = new InventoryItemSlot<TTemplate, TInstance>(this);
            }
        }

        [System.Obsolete("Use AddItem instead")]
        public void AddItemByID(int id)
        {
            //@deprecated
        }

        public void AddItem(string name, int amount = 1)
        {
            TTemplate itemTemplate = dataTempateManager.FindDataTemplate(name);
            Debug.Log(itemTemplate);
            if (itemTemplate != null)
            {
                TInstance itemInstance = new TInstance
                {
                    Template = itemTemplate,
                    Amount = amount
                };
                AddItemInstance(itemInstance);
            }
        }

        public void AddItem(TTemplate itemTemplate, int amount = 1)
        {
            if (itemTemplate != null)
            {
                TInstance itemInstance = new TInstance
                {
                    Template = itemTemplate,
                    Amount = amount
                };
                AddItemInstance(itemInstance);
            }
        }

        public void AddItemInstance(TInstance itemInstance)
        {
            bool stacked = StackItem(itemInstance);
            if (!stacked)
            {
                bool added = AddNewItem(itemInstance);
                if (added)
                {

                }
            }
        }

        public bool RemoveItems(TInstance itemInstance)
        {
            if (!HasItem(itemInstance))
            {
                Debug.Log($"don't have {itemInstance.Amount} {itemInstance.Template.name} to remove");
                return false;
            }

            InventoryBaseSlot<TTemplate, TInstance>[] slots = Array.FindAll(ItemSlots, slot => slot.ContainsItem && slot.Item.Template.name == itemInstance.Template.name);

            int amtRemainingToRemove = itemInstance.Amount;
            foreach (var slot in slots)
            {
                int removedAmt = slot.RemoveItem(amtRemainingToRemove);
                amtRemainingToRemove -= removedAmt;
                if (amtRemainingToRemove == 0)
                {
                    break;
                }
            }
            return true;
        }

        public void RemoveAllItems()
        {
            foreach (var slot in ItemSlots)
            {
                slot.SetItemInstance(null);
            }
        }

        public void FindAllItems(List<TInstance> items, System.Predicate<TInstance> predicate)
        {
            InventoryBaseSlot<TTemplate, TInstance>[] slots = Array.FindAll(ItemSlots, slot => slot.ContainsItem && predicate(slot.Item));

            foreach (var slot in slots)
            {
                items.Add(slot.Item);
            }
        }

        public bool HasItem(TInstance itemInstance)
        {
            ItemTemplate itemTemplate = itemInstance.Template;
            InventoryBaseSlot<TTemplate, TInstance>[] slots = Array.FindAll(ItemSlots, slot => slot.ContainsItem && slot.Item.Template.name == itemTemplate.name);
            int availableAmt = 0;
            foreach (var slot in slots)
            {
                availableAmt += slot.Item.Amount;
            }

            return availableAmt >= itemInstance.Amount;
        }

        public bool IsFull()
        {
            for (int i = 0; i < SlotCount; i++)
            {
                if (!ItemSlots[i].ContainsItem) { return false; }
            }
            return true;
        }

        private bool StackItem(TInstance newItem)
        {
            ItemTemplate itemTemplate = newItem.Template;

            if (!itemTemplate.IsStackable)
            {
                return false;
            }

            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot<TTemplate, TInstance> slot = ItemSlots[i];
                if (slot.HasItem(newItem.Template))
                {
                    if (slot.Item.Template == itemTemplate)
                    {
                        // @todo: stack items
                        //  slot.StackItem (amount);
                        OnItemAdded(newItem);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AddNewItem(TInstance newItem)
        {
            ItemTemplate itemTemplate = newItem.Template;

            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot<TTemplate, TInstance> slot = ItemSlots[i];

                if (!slot.ContainsItem)
                {
                    slot.SetItemInstance(newItem);

                    // @todo: auto equip

                    OnItemAdded(newItem);
                    Debug.Log("item added " + newItem.Template.name);
                    return true;
                }
            }

            return false;
        }
    }
}