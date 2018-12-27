using uItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace uInventory
{
    public class InventoryUI<TTemplate, TInstance> : MonoBehaviour
    where TTemplate : ItemTemplate, new ()
    where TInstance : ItemInstance<TTemplate>, new ()
    {
        [Header ("[Prefabs]")]
        public GameObject slotUIPrefab;

        private Inventory<TTemplate, TInstance> inventory;
        private InventoryUIManager<TTemplate, TInstance> inventoryManager;

        private void Awake ()
        {
            Assert.IsNotNull (slotUIPrefab, "didn't provide inventory slot prefab");
        }

        public void SetInventory (Inventory<TTemplate, TInstance> inventory, InventoryUIManager<TTemplate, TInstance> inventoryManager)
        {
            Assert.IsNotNull (inventory, "null inventory is passed to inventory ui");
            Assert.IsNotNull (inventoryManager, "null inventory manager is passed to inventory ui");

            this.inventory = inventory;
            this.inventoryManager = inventoryManager;
            InitSlots ();
        }

        private void InitSlots ()
        {
            Assert.IsNotNull (inventory, "can't initialize inventory ui without inventory model");

            // initialize item slots
            for (int i = 0; i < inventory.SlotCount; i++)
            {
                InventoryItemSlot<TTemplate, TInstance> slot = inventory.ItemSlots[i];

                InventoryItemSlotUI<TTemplate, TInstance> slotUI = Instantiate (slotUIPrefab, transform).GetComponent<InventoryItemSlotUI<TTemplate, TInstance>> ();
                slotUI.SetSlot (slot);
                RegisterUIEvents (slotUI);
            }
        }

        private void RegisterUIEvents (InventoryBaseSlotUI<TTemplate, TInstance> slotUI)
        {
            slotUI.OnLeftClickSlot += LeftClickSlot;
            slotUI.OnRightClickSlot += RightClickSlot;
            slotUI.OnMouseDragSlot += DragSlot;
            slotUI.OnMouseEnterSlot += MouseEnterSlot;
            slotUI.OnMouseExitSlot += MouseExitSlot;
        }

        private void MouseEnterSlot (InventoryBaseSlot<TTemplate, TInstance> slot)
        {
            // @todo: hover
        }

        private void MouseExitSlot (InventoryBaseSlot<TTemplate, TInstance> slot)
        {
            // @todo: unhover
        }

        private void LeftClickSlot (InventoryBaseSlot<TTemplate, TInstance> slot)
        {
            // @todo: consume, equip, etc

            if (inventoryManager.IsDraggingItem)
            {
                // swap item with dragged slot if they are different items. otherwise, we might be able to stack them
                if (slot.ContainsItem && slot.Item.Template != inventoryManager.DraggedItem.Template)
                {
                    inventoryManager.DraggedSlot.SetItemInstance (slot.Item);
                }

                inventoryManager.PutDraggedItem (slot);
            }
        }

        private void RightClickSlot (InventoryBaseSlot<TTemplate, TInstance> slot) { }

        private void DragSlot (InventoryBaseSlot<TTemplate, TInstance> slot)
        {
            if (inventoryManager.IsDraggingItem)
            {
                // @todo: swap dragged item
            }
            else
            {
                if (!slot.ContainsItem) { return; }

                inventoryManager.SetDraggedItem (slot);
            }
        }
    }
}