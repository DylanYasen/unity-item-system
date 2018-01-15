using System;
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

        public delegate void ItemChangedDelegate (Item item, int amount);
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
            Item item = itemDatabase.GetItemByName (name);
            if (item != null)
            {
                AddItem (ScriptableObject.Instantiate (item), amount);
            }
        }

        public void AddItem (Item item, int amount = 1)
        {
            bool stacked = StackItem (item, amount);
            if (!stacked)
            {
                bool added = AddNewItem (item, amount);
                if (added) { }
                else
                {
                    // @todo: notify failed to add
                }
            }
        }

        public bool RemoveItems (Item item, int amt = 1)
        {
            if (!HasItem (item, amt))
            {
                Debug.LogErrorFormat ("don't have %d $s to remove", amt, item.name);
                return false;
            }

            InventoryBaseSlot[] slots = Array.FindAll (ItemSlots, slot => slot.ContainsItem && slot.Item.name == item.name);

            int amtRemainingToRemove = amt;
            foreach (var slot in slots)
            {
                int removedAmt = slot.RemoveItem (amtRemainingToRemove);
                amtRemainingToRemove -= removedAmt;
                if (amtRemainingToRemove == 0)
                {
                    break;
                }
            }
            return true;
        }

        public void RemoveAllItems ()
        {
            foreach (var slot in ItemSlots)
            {
                slot.SetItem (null);
            }
        }

        public Item[] FindAllItems (System.Predicate<Item> predicate)
        {
            InventoryBaseSlot[] slots = Array.FindAll (ItemSlots, slot => slot.ContainsItem && predicate (slot.Item));

            // @todo: optimize allocation
            Item[] items = new Item[slots.Length];
            for (int i = 0; i < slots.Length; i++)
            {
                items[i] = slots[i].Item;
            }
            return items;
        }

        public bool HasItem (Item item, int amt = 1)
        {
            InventoryBaseSlot[] slots = Array.FindAll (ItemSlots, slot => slot.ContainsItem && slot.Item.name == item.name);
            int availableAmt = 0;
            foreach (var slot in slots)
            {
                availableAmt += slot.ItemAmount;
            }

            return availableAmt >= amt;
        }

        private bool StackItem (Item newItem, int amount)
        {
            if (!newItem.IsStackable)
            {
                return false;
            }

            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot slot = ItemSlots[i];
                if (slot.HasItem (newItem))
                {
                    slot.StackItem (amount);
                    return true;
                }
            }

            return false;
        }

        private bool AddNewItem (Item newItem, int amount)
        {
            for (int i = 0; i < ItemSlots.Length; i++)
            {
                InventoryItemSlot slot = ItemSlots[i];

                if (!slot.ContainsItem)
                {
                    slot.SetItem (newItem, amount);

                    // @todo: auto equip

                    OnItemAdded (newItem, amount);
                    return true;
                }
            }

            return false;
        }
    }
}