using UnityEngine;
using System.Collections;
using uItem;

namespace uInventory
{
    public abstract class InventoryBaseSlot
    {
        public bool ContainsItem { get { return Item != null; } }
        public Item Item { get; protected set; }
        public int ItemAmount { get; private set; }
        public Inventory OwningInventory { get; private set; }

        public event Inventory.ItemChangedDelegate OnItemChanged = delegate { };

        public InventoryBaseSlot(Inventory inventory)
        {
            OwningInventory = inventory;
        }

        public virtual bool SetItem(Item item, int amount = 0)
        {
            this.Item = item;
            this.ItemAmount = amount;

            OnItemChanged(item, amount);

            return true;
        }

    }
}