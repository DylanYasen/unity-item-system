using System.Collections;
using uItem;
using UnityEngine;
using UnityEngine.Assertions;

namespace uInventory
{
    public abstract class InventoryBaseSlot
    {
        public bool ContainsItem { get { return itemInstance.Template != null && itemInstance.Amount > 0; } }
        public ItemInstance Item { get { return itemInstance; } }
        public Inventory OwningInventory { get; private set; }

        public event Inventory.ItemChangedDelegate OnItemChanged = delegate { };

        protected ItemInstance itemInstance; // { get; protected set; }

        public InventoryBaseSlot (Inventory inventory)
        {
            OwningInventory = inventory;
        }

        public virtual bool SetItem (ItemTemplate itemTemplate, int amount = 0)
        {
            itemInstance.Template = itemTemplate;
            itemInstance.Amount = amount;

            OnItemChanged (itemInstance);

            return true;
        }

        // public virtual bool SetItem (Item item, int amount = 0)

        public virtual bool SetItemInstance (ItemInstance itemInstance)
        {
            ItemTemplate template = itemInstance.Template;
            if(template == Item.Template && template.IsStackable)
            {
                StackItem(itemInstance.Amount);
                return true;
            }

            this.itemInstance = itemInstance;
            OnItemChanged (this.itemInstance);

            return true;
        }

        public virtual void StackItem (int amt)
        {
            // @todo: handle overflow
            itemInstance.Amount += amt;

            OnItemChanged (itemInstance);
        }

        public virtual bool HasItem (ItemTemplate template)
        {
            return (ContainsItem && Item.Template == template);
        }

        public virtual int RemoveItem (int amt = 1)
        {
            Assert.IsTrue (itemInstance.Amount > 0, "no items in the slot to remove");

            int availableToRemove = itemInstance.Amount;

            itemInstance.Amount -= amt;
            if (itemInstance.Amount < 1)
            {
                itemInstance.Clear();
            }
            OnItemChanged (itemInstance);

            return availableToRemove;
        }
    }
}