using System.Collections;
using uItem;
using UnityEngine;
using UnityEngine.Assertions;

namespace uInventory
{
    public abstract class InventoryBaseSlot<TTemplate, TInstance>
        where TTemplate : ItemTemplate, new ()
    where TInstance : ItemInstance<TTemplate>, new ()
    {
        public bool ContainsItem { get { return itemInstance != null && itemInstance.Amount > 0; } }
        public TInstance Item { get { return itemInstance; } }
        public Inventory<TTemplate, TInstance> OwningInventory { get; private set; }

        public event Inventory<TTemplate, TInstance>.ItemChangedDelegate OnItemChanged = delegate { };

        protected TInstance itemInstance; // { get; protected set; }

        public InventoryBaseSlot (Inventory<TTemplate, TInstance> inventory)
        {
            OwningInventory = inventory;
        }

        public virtual bool SetItem (TTemplate itemTemplate, int amount = 0)
        {
            itemInstance.Template = itemTemplate;
            itemInstance.Amount = amount;

            OnItemChanged (itemInstance);

            return true;
        }

        public virtual bool SetItemInstance (TInstance itemInstance)
        {
            if (itemInstance != null)
            {
                TTemplate template = itemInstance.Template;
                if (ContainsItem && template == Item.Template && template.IsStackable)
                {
                    StackItem (itemInstance.Amount);
                    return true;
                }
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

        public virtual bool HasItem (TTemplate template)
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
                itemInstance = null;
            }
            OnItemChanged (itemInstance);

            return availableToRemove;
        }
    }
}