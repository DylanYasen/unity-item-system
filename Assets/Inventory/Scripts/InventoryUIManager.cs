using uItem;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace uInventory
{
    public class InventoryUIManager : MonoBehaviour
    {
        public bool IsDraggingItem { get { return !DraggedItem.IsEmpty(); } }
        public InventoryBaseSlot DraggedSlot { get; private set; }

        [Header("[UI]")]
        public Image dragItemImage;

        public ItemInstance DraggedItem; 
        private Canvas canvas;

        private void Awake()
        {
            canvas = transform.GetComponent<Canvas>();

            Assert.IsNotNull(dragItemImage, "didn't provide dragItemImage");
            dragItemImage.enabled = false;
        }

        public void SetDraggedItem(InventoryBaseSlot draggedSlot)
        {
            DraggedSlot = draggedSlot;
            DraggedItem = draggedSlot.Item;

            draggedSlot.SetItem(null);

            dragItemImage.sprite = DraggedItem.Template.icon;
            dragItemImage.enabled = true;
        }

        public void PutDraggedItem(InventoryBaseSlot slot)
        {
            if (slot.SetItemInstance(DraggedItem))
            {
                DraggedItem.Clear();
                DraggedSlot = null;
                dragItemImage.enabled = false;
            }
        }

        private void Update()
        {
            if (IsDraggingItem)
            {
                Vector3 pos = Input.mousePosition;
                dragItemImage.transform.position = new Vector2(pos.x, pos.y);

                // put back item that's currently being dragged
                if (Input.GetMouseButtonDown(1))
                {
                    PutDraggedItem(DraggedSlot);
                }
            }
        }
    }
}