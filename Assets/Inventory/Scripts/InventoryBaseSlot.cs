using System.Collections;
using uItem;
using UnityEngine;
using UnityEngine.Assertions;

namespace uInventory
{
    public abstract class InventoryBaseSlot
    {
        public bool ContainsItem { get { return Item != null; } }
        public Item Item { get; protected set; }
        public int ItemAmount { get; private set; }
        public Inventory OwningInventory { get; private set; }

        public event Inventory.ItemChangedDelegate OnItemChanged = delegate { };

        public InventoryBaseSlot (Inventory inventory)
        {
            OwningInventory = inventory;
        }

        public virtual bool SetItem (Item item, int amount = 0)
        {
            this.Item = item;
            this.ItemAmount = amount;

            OnItemChanged (item, amount);

            return true;
        }

        public virtual void StackItem (int amt)
        {
            this.ItemAmount += amt;

            OnItemChanged (Item, ItemAmount);
        }

        public virtual bool HasItem (Item item)
        {
            return (ContainsItem && Item.name == item.name);
        }

        public virtual int RemoveItem (int amt = 1)
        {
            Assert.IsTrue (ItemAmount > 0, "no items in the slot to remove");

            int availableToRemove = ItemAmount;

            ItemAmount -= amt;
            if (ItemAmount < 1)
            {
                Item = null;
            }
            OnItemChanged (Item, ItemAmount);

            return availableToRemove;
        }
    }
}