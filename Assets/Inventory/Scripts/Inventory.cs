using System.Collections;
using System.Collections.Generic;
using uItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace uInventory
{
    public class Inventory
    {
        public InventoryItemSlot[] ItemSlots { get; private set; }
        public int SlotCount { get; private set; }

        public delegate void ItemChangedDelegate (ItemInstance itemInstance);
        public event ItemChangedDelegate OnItemAdded = delegate { };

        private GameObject owner;
        private ItemDatabase itemDatabase;

        public Inventory (GameObject inOwner, ItemDatabase inItemDatabase, int slotCount)
        {
            this.owner = inOwner;
            this.SlotCount = slotCount;
            this.itemDatabase = inItemDatabase;

            // initialize item slots
            ItemSlots = new InventoryItemSlot[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                ItemSlots[i] = new InventoryItemSlot (this);
            }
        }

        [System.Obsolete ("Use AddItem instead")]
        public void AddItemByID (int id)
        {
            //@deprecated
        }

        public void AddItem (string name, int amount = 1)
        {
            ItemTemplate itemTemplate = itemDatabase.GetItemByName (name);
            if (itemTemplate != null)
            {
                ItemInstance itemInstance;
                itemInstance.Template = itemTemplate;
                itemInstance.Amount = amount;

                // AddItem(ScriptableObject.Instantiate(item), amount);
                AddItemInstance (itemInstance);
            }
        }

        public void AddItem (ItemTemplate itemTemplate, int amount = 1)
        {
            if (itemTemplate != null)
            {
                ItemInstance itemInstance;
                itemInstance.Template = itemTemplate;
                itemInstance.Amount = amount;

                // AddItem(ScriptableObject.Instantiate(item), amount);
                AddItemInstance (itemInstance);
            }
        }

        public void AddItemInstance (ItemInstance itemInstance)
        {
            bool stacked = StackItem (itemInstance);
            if (!stacked)
            {
                bool added = AddNewItem (itemInstance);
                if (added) { 
                    
                }
                else
                {
                    // @todo: notify failed to add
                }
            }
        }

        public void RemoveAllItems ()
        {
            foreach (var slot in ItemSlots)
            {
                slot.SetItem (null);
            }
        }

        private bool StackItem (ItemInstance newItem)
        {
            ItemTemplate itemTemplate = newItem.Template;

            if (!itemTemplate.IsStackable)
            {
                return false;
            }

            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot slot = ItemSlots[i];
                if (slot.ContainsItem)
                {
                    if (slot.Item.Template == itemTemplate)
                    {
                        // @todo: stack items
                        OnItemAdded (newItem);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AddNewItem (ItemInstance newItem)
        {
            ItemTemplate itemTemplate = newItem.Template;

            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot slot = ItemSlots[i];

                if (!slot.ContainsItem)
                {
                    slot.SetItemInstance (newItem);

                    // @todo: auto equip

                    OnItemAdded (newItem);
                    return true;
                }
            }

            return false;
        }
    }
}