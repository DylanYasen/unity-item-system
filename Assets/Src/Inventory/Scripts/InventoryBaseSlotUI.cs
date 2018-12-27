using uItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace uInventory
{
    [RequireComponent (typeof (Image))]
    public abstract class InventoryBaseSlotUI<TTemplate, TInstance> : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
    where TTemplate : ItemTemplate, new ()
    where TInstance : ItemInstance<TTemplate>, new ()
    {
        public delegate void SlotInteractionDelegate (InventoryBaseSlot<TTemplate, TInstance> slot);
        public event SlotInteractionDelegate OnLeftClickSlot = delegate { };
        public event SlotInteractionDelegate OnRightClickSlot = delegate { };
        public event SlotInteractionDelegate OnMouseEnterSlot = delegate { };
        public event SlotInteractionDelegate OnMouseExitSlot = delegate { };
        public event SlotInteractionDelegate OnMouseDragSlot = delegate { };

        protected InventoryBaseSlot<TTemplate, TInstance> slot;

        public Image BackgroundImage;
        public Image ItemImage;

        protected virtual void Awake ()
        {
            BackgroundImage = GetComponent<Image> ();
            ItemImage = gameObject.transform.Find ("ItemImage").GetComponent<Image> ();

            Assert.IsNotNull (BackgroundImage, "Inventory slot doesn't have an image component");
            Assert.IsNotNull (ItemImage, "Inventory slot doesn't have a child component called 'ItemImage' with image component");

            ItemImage.enabled = false;
        }

        protected virtual void OnEnable ()
        {
            // @todo: better to set before becoming visiable
            if (slot != null)
            {
                UpdateUI (slot.Item);
            }
        }

        public virtual void SetSlot (InventoryBaseSlot<TTemplate, TInstance> slot)
        {
            Assert.IsNotNull (slot, "null inventory slot model is passed to inventory slot ui");

            this.slot = slot;
            this.slot.OnItemChanged += UpdateUI;
        }

        protected virtual void UpdateUI (TInstance Item)
        {
            if (Item != null)
            {
                ItemImage.sprite = Item.Template.icon;
                ItemImage.enabled = true;
            }
            else
            {
                ItemImage.enabled = false;
                ItemImage.sprite = null;
            }
        }

        public void OnPointerDown (PointerEventData eventData)
        {
            if (slot == null) { return; }

            PointerEventData.InputButton button = eventData.button;
            if (button == PointerEventData.InputButton.Left)
            {
                OnLeftClickSlot (slot);
            }
            else if (button == PointerEventData.InputButton.Right)
            {
                OnRightClickSlot (slot);
            }
        }

        public virtual void OnPointerEnter (PointerEventData eventData)
        {
            OnMouseEnterSlot (slot);
        }

        public virtual void OnPointerExit (PointerEventData eventData)
        {
            OnMouseExitSlot (slot);
        }

        public virtual void OnDrag (PointerEventData eventData)
        {
            OnMouseDragSlot (slot);
        }
    }
}