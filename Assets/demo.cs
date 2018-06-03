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
	}
}