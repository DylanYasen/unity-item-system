using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using uItem;

namespace uInventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("[Prefabs]")]
        public GameObject slotUIPrefab;

        private Inventory inventory;
        private InventoryUIManager inventoryManager;

        private void Awake()
        {
            Assert.IsNotNull(slotUIPrefab, "didn't provide inventory slot prefab");
        }

        public void SetInventory(Inventory inventory, InventoryUIManager inventoryManager)
        {
            Assert.IsNotNull(inventory, "null inventory is passed to inventory ui");
            Assert.IsNotNull(inventoryManager, "null inventory manager is passed to inventory ui");

            this.inventory = inventory;
            this.inventoryManager = inventoryManager;
            InitSlots();
        }

        private void InitSlots()
        {
            Assert.IsNotNull(inventory, "can't initialize inventory ui without inventory model");

            // initialize item slots
            for (int i = 0; i < inventory.SlotCount; i++)
            {
                InventoryItemSlot slot = inventory.ItemSlots[i];

                InventoryItemSlotUI slotUI = Instantiate(slotUIPrefab, transform).GetComponent<InventoryItemSlotUI>();
                slotUI.SetSlot(slot);
                RegisterUIEvents(slotUI);
            }
        }

        private void RegisterUIEvents(InventoryBaseSlotUI slotUI)
        {
            slotUI.OnLeftClickSlot += LeftClickSlot;
            slotUI.OnRightClickSlot += RightClickSlot;
            slotUI.OnMouseDragSlot += DragSlot;
            slotUI.OnMouseEnterSlot += MouseEnterSlot;
            slotUI.OnMouseExitSlot += MouseExitSlot;
        }

        private void MouseEnterSlot(InventoryBaseSlot slot)
        {
            // @todo: hover
        }

        private void MouseExitSlot(InventoryBaseSlot slot)
        {
            // @todo: unhover
        }

        private void LeftClickSlot(InventoryBaseSlot slot)
        {
            // @todo: consume, equip, etc

            if (inventoryManager.IsDraggingItem)
            {
                // swap item with dragged slot
                if (slot.ContainsItem)
                {
                    inventoryManager.DraggedSlot.SetItem(slot.Item);
                }

                inventoryManager.PutDraggedItem(slot);
            }
        }

        private void RightClickSlot(InventoryBaseSlot slot)
        {
        }

        private void DragSlot(InventoryBaseSlot slot)
        {
            if (inventoryManager.IsDraggingItem)
            {
                // @todo: swap dragged item
            }
            else
            {
                if (!slot.ContainsItem) { return; }

                inventoryManager.SetDraggedItem(slot);
            }
        }
    }
}