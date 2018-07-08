using System.Collections;
using uItem;
using UnityEngine;

namespace uInventory
{
    public class InventoryItemSlot<TTemplate, TInstance> : InventoryBaseSlot<TTemplate, TInstance>
    where TTemplate : ItemTemplate, new ()
    where TInstance : ItemInstance<TTemplate>, new ()
    {
        public InventoryItemSlot (Inventory<TTemplate, TInstance> inventory) : base (inventory) { }
    }
}