using System.Collections;
using System.Collections.Generic;
using uInventory;
using uItem;
using UnityEngine;

public class demo : MonoBehaviour
{
	public InventoryUIManager inventoryUIManager;
	public InventoryUI inventoryUI;
	public ItemDatabase itemDatabase;

	public Inventory inventory;

	void Start ()
	{
		inventory = new Inventory (gameObject, itemDatabase, 12);
		inventoryUI.SetInventory (inventory, inventoryUIManager);
		inventory.AddItem ("Unicorn Icecream", 4);
		inventory.AddItem ("Icecream", 4);

		ItemTemplate template = itemDatabase.GetItemByName ("Icecream");
		inventory.ItemSlots[4].SetItem (template, 2);
	}
}