using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace uItem
{
    public class ItemDatabase : MonoBehaviour
    {
        public Dictionary<string, Item> ItemRegistry { get; private set; }

        void Awake()
        {
            LoadItems();
        }

        public void LoadItems()
        {
            ItemRegistry = new Dictionary<string, Item>();

            Item[] items = Resources.LoadAll<Item>("items");
            for (int i = 0; i < items.Length; i++)
            {
                Item item = items[i];
                if (!ItemRegistry.ContainsKey(item.name))
                {
                    ItemRegistry.Add(item.name, item);
                }
            }
        }

        public Item GetItemByName(string itemName)
        {
            Assert.IsTrue(ItemRegistry.ContainsKey(itemName), string.Format("item database doesn't have the requested item: %s\n ", itemName));
            return ItemRegistry[itemName];
        }
    }
}
