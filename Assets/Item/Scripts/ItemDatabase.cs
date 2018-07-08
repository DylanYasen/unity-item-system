using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace uItem
{
    public class ItemDatabase<T> : MonoBehaviour where T : ItemTemplate
    {
        public string ItemAssetsRootPath = "Items";
        public Dictionary<string, T> ItemRegistry { get; private set; }

        protected void Awake ()
        {
            LoadItems ();
        }

        public void LoadItems ()
        {
            ItemRegistry = new Dictionary<string, T> ();

            T[] items = Resources.LoadAll<T> (ItemAssetsRootPath);
            for (int i = 0; i < items.Length; i++)
            {
                T item = items[i];
                if (!ItemRegistry.ContainsKey (item.name))
                {
                    Debug.Log (item.name);
                    ItemRegistry.Add (item.name, item);
                }
            }
        }

        public T GetItemByName (string itemName)
        {
            Assert.IsTrue (ItemRegistry.ContainsKey (itemName), string.Format ("item database doesn't have the requested item: {0}\n ", itemName));
            return ItemRegistry[itemName];
        }

        public T[] FindAllItems (System.Predicate<T> predicate)
        {
            T[] matches = ItemRegistry.Where (pair => predicate (pair.Value))
                .Select (pair => pair.Value).ToArray ();

            return matches;
        }
    }
}