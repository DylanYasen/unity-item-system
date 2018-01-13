using UnityEngine;
using System;
using System.Collections;

namespace uItem
{
    public class Item : ScriptableObject
    {
        [Flags]
        public enum ItemProperty
        {
            Sellable = 1 << 0,
            Purchasable = 1 << 1,
            Droppable = 1 << 2,
            Stackable = 1 << 3,
            Consumeable = 1 << 4,
        }

        public bool IsStackable { get { return (properties & ItemProperty.Stackable) != 0; } }

        [Header("[Info]")]
        public string description;
        public GameObject lootPrefab;

        [Header("[UI]")]
        public Sprite icon;

        [Header("[Property]")]
        public ItemProperty properties; // @todo: draw enum flag UI
        //public Modifier[] modifiers;
    }
}