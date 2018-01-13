using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Assertions;
using uItem;

namespace uInventory
{
    public class Inventory
    {
        public InventoryItemSlot[] ItemSlots { get; private set; }
        public int SlotCount { get; private set; }

        public delegate void ItemChangedDelegate(Item item, int amount);
        public event ItemChangedDelegate OnItemAdded = delegate { };

        private GameObject owner;
        private ItemDatabase itemDatabase;

        public Inventory(GameObject inOwner, ItemDatabase inItemDatabase, int slotCount)
        {
            this.owner = inOwner;
            this.SlotCount = slotCount;
            this.itemDatabase = inItemDatabase;

            // initialize item slots
            ItemSlots = new InventoryItemSlot[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                ItemSlots[i] = new InventoryItemSlot(this);
            }
        }

        [System.Obsolete("Use AddItem instead")]
        public void AddItemByID(int id)
        {
            //@deprecated
        }

        public void AddItem(string name, int amount = 1)
        {
            Item item = itemDatabase.GetItemByName(name);
            if (item != null)
            {
                AddItem(ScriptableObject.Instantiate(item), amount);
            }
        }

        public void AddItem(Item item, int amount = 1)
        {
            bool stacked = StackItem(item, amount);
            if (!stacked)
            {
                bool added = AddNewItem(item, amount);
                if (added)
                {
                }
                else
                {
                    // @todo: notify failed to add
                }
            }
        }

        public void RemoveAllItems()
        {
            foreach (var slot in ItemSlots)
            {
                slot.SetItem(null);
            }
        }

        private bool StackItem(Item newItem, int amount)
        {
            if (!newItem.IsStackable)
            {
                return false;
            }

            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot slot = ItemSlots[i];
                if (slot.ContainsItem)
                {
                    if (slot.Item.name == newItem.name)
                    {
                        // @todo: stack items
                        OnItemAdded(newItem, amount);
                        return true;
                    }
                }
            }

            return false;
        }

        private bool AddNewItem(Item newItem, int amount)
        {
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot slot = ItemSlots[i];

                if (!slot.ContainsItem)
                {
                    slot.SetItem(newItem, amount);

                    // @todo: auto equip

                    OnItemAdded(newItem, amount);
                    return true;
                }
            }

            return false;
        }
    }
}
