using uItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace uInventory
{
    public class InventoryUIManager<TTemplate, TInstance> : MonoBehaviour
    where TTemplate : ItemTemplate, new ()
    where TInstance : ItemInstance<TTemplate>, new ()
    {
        public bool IsDraggingItem { get { return (DraggedItem != null && !DraggedItem.IsEmpty ()); } }
        public InventoryBaseSlot<TTemplate, TInstance> DraggedSlot { get; private set; }

        [Header ("[UI]")]
        public Image dragItemImage;

        public TInstance DraggedItem;
        private Canvas canvas;

        private void Awake ()
        {
            canvas = transform.GetComponent<Canvas> ();

            Assert.IsNotNull (dragItemImage, "didn't provide dragItemImage");
            dragItemImage.enabled = false;
        }

        public void SetDraggedItem (InventoryBaseSlot<TTemplate, TInstance> draggedSlot)
        {
            DraggedSlot = draggedSlot;
            DraggedItem = draggedSlot.Item;

            draggedSlot.SetItemInstance (null);

            dragItemImage.sprite = DraggedItem.Template.icon;
            dragItemImage.enabled = true;
        }

        public void PutDraggedItem (InventoryBaseSlot<TTemplate, TInstance> slot)
        {
            if (slot.SetItemInstance (DraggedItem))
            {
                DraggedItem = null;
                DraggedSlot = null;
                dragItemImage.enabled = false;
            }
        }

        private void Update ()
        {
            if (IsDraggingItem)
            {
                Vector3 pos = Input.mousePosition;
                dragItemImage.transform.position = new Vector2 (pos.x, pos.y);

                // put back item that's currently being dragged
                if (Input.GetMouseButtonDown (1))
                {
                    PutDraggedItem (DraggedSlot);
                }
            }
        }
    }
}