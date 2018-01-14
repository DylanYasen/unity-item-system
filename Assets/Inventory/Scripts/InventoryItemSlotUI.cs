using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using uItem;

namespace uInventory
{
    public class InventoryItemSlotUI : InventoryBaseSlotUI
    {
        public Text ItemAmountText;

        protected override void Awake()
        {
            base.Awake();

            Assert.IsNotNull(ItemAmountText, "Inventory item slot doesn't have a child component called 'ItemAmountText' with text component");
        }

        public override void SetSlot(InventoryBaseSlot inSlot)
        {
            base.SetSlot(inSlot);
        }

        protected override void UpdateUI(Item item, int amount)
        {
            base.UpdateUI(item, amount);

            if (item != null)
            {
                if (item.IsStackable && amount > 1)
                {
                    ItemAmountText.text = amount.ToString();
                    ItemAmountText.enabled = true;
                }
            }
            else
            {
                ItemAmountText.enabled = false;
                ItemAmountText.text = null;
            }
        }
    }
}