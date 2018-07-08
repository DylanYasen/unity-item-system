using uItem;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace uInventory
{
    public class InventoryItemSlotUI<TTemplate, TInstance> : InventoryBaseSlotUI<TTemplate, TInstance>
        where TTemplate : ItemTemplate, new ()
    where TInstance : ItemInstance<TTemplate>, new ()
    {
        public Text ItemAmountText;

        protected override void Awake ()
        {
            base.Awake ();

            Assert.IsNotNull (ItemAmountText, "Inventory item slot doesn't have a child component called 'ItemAmountText' with text component");
        }

        public override void SetSlot (InventoryBaseSlot<TTemplate, TInstance> inSlot)
        {
            base.SetSlot (inSlot);
        }

        protected override void UpdateUI (TInstance item)
        {
            base.UpdateUI (item);

            if (item != null)
            {
                if (item.Template.IsStackable && item.Amount > 1)
                {
                    ItemAmountText.text = item.Amount.ToString ();
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