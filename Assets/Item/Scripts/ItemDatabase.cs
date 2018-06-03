using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace uItem
{
    public class ItemDatabase : MonoBehaviour
    {
        public Dictionary<string, ItemTemplate> ItemRegistry { get; private set; }

        void Awake ()
        {
            LoadItems ();
        }

        public void LoadItems ()
        {
            ItemRegistry = new Dictionary<string, ItemTemplate>();

            ItemTemplate[] items = Resources.LoadAll<ItemTemplate>("Items");
            for (int i = 0; i < items.Length; i++)
            {
                ItemTemplate item = items[i];
                if (!ItemRegistry.ContainsKey(item.name))
                {
                    Debug.Log(item.name);
                    ItemRegistry.Add (item.name, item);
                }
            }
            
        }

        public ItemTemplate GetItemByName(string itemName)
        {
            Assert.IsTrue (ItemRegistry.ContainsKey (itemName), string.Format ("item database doesn't have the requested item: {0}\n ", itemName));
            return ItemRegistry[itemName];
        }

        // public Item[] FindAllItems (System.Predicate<Item> predicate)
        // {
        //     Item[] matches = ItemRegistry.Where (pair => predicate (pair.Value))
        //         .Select (pair => pair.Value).ToArray();

        //     return matches;
        // }
    }
}