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

        protected override void UpdateUI(ItemInstance item)
        {
            base.UpdateUI(item);

            if (item.Template != null)
            {
                if (item.Template.IsStackable && item.Amount > 1)
                {
                    ItemAmountText.text = item.Amount.ToString();
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