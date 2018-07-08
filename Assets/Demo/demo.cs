using System.Collections;
using System.Collections.Generic;
using uInventory;
using uItem;
using UnityEngine;

public class demo : MonoBehaviour
{
	public InventoryUIManager<ItemTemplate, ItemInstance<ItemTemplate>> inventoryUIManager;
	public InventoryUI<ItemTemplate, ItemInstance<ItemTemplate>> inventoryUI;
	public ItemDatabase<ItemTemplate> itemDatabase;

	public Inventory<ItemTemplate, ItemInstance<ItemTemplate>> inventory;

	void Start ()
	{
		inventory = new Inventory<ItemTemplate, ItemInstance<ItemTemplate>> (gameObject, itemDatabase, 12);
		inventoryUI.SetInventory (inventory, inventoryUIManager);
		inventory.AddItem ("Unicorn Icecream", 4);
		inventory.AddItem ("Icecream", 4);

		ItemTemplate template = itemDatabase.GetItemByName ("Icecream");
		inventory.ItemSlots[4].SetItem (template, 2);
	}
}