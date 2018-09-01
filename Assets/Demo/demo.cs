using System.Collections;
using System.Collections.Generic;
using uInventory;
using uItem;
using UnityEngine;

namespace Demo
{
    public class demo : MonoBehaviour
    {
        public InventoryUIManager inventoryUIManager;
        public InventoryUI inventoryUI;

        public Inventory<ItemTemplate, ItemInstance<ItemTemplate>> inventory;

        void Start()
        {
            inventory = new Inventory<ItemTemplate, ItemInstance<ItemTemplate>>(gameObject, ItemTemplateManager.Instance, 12);
            inventoryUI.SetInventory(inventory, inventoryUIManager);
            inventory.AddItem("Unicorn Icecream", 4);
            inventory.AddItem("Icecream", 4);

            ItemTemplate template = ItemTemplateManager.Instance.FindDataTemplate("Icecream");
            inventory.ItemSlots[4].SetItem(template, 2);
        }
    }
}