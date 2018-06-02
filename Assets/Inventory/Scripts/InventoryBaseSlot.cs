using System.Collections;
using uItem;
using UnityEngine;

namespace uInventory
{
    public abstract class InventoryBaseSlot
    {
        public bool ContainsItem { get { return ItemInstance.Template != null || ItemInstance.Amount < 1; } }
        public ItemInstance Item { get { return ItemInstance; } }
        public Inventory OwningInventory { get; private set; }

        public event Inventory.ItemChangedDelegate OnItemChanged = delegate { };

        protected ItemInstance ItemInstance; // { get; protected set; }

        public InventoryBaseSlot (Inventory inventory)
        {
            OwningInventory = inventory;
        }

        public virtual bool SetItem (ItemTemplate itemTemplate, int amount = 0)
        {
            ItemInstance.Template = itemTemplate;
            ItemInstance.Amount = amount;

            OnItemChanged (ItemInstance);

            return true;
        }

        public virtual bool SetItemInstance (ItemInstance itemInstance)
        {
            ItemInstance = itemInstance;

            OnItemChanged (ItemInstance);

            return true;
        }

    }
}