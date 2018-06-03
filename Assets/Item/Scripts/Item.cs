using System;
using System.Collections;
using UnityEngine;

namespace uItem
{
    public class ItemTemplate : ScriptableObject
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
        public bool IsPurchaseable { get { return (properties & ItemProperty.Purchasable) != 0; } }
        public bool IsSellable { get { return (properties & ItemProperty.Sellable) != 0; } }

        [Header ("[Info]")]
        public string description;
        public GameObject lootPrefab;

        [Header ("[UI]")]
        public Sprite icon;

        [Header ("[Property]")]
<<<<<<< HEAD
=======
        [EnumFlags]
>>>>>>> e62ccbeb4c037d6c3fd441996bb24446e58d46f0
        public ItemProperty properties; // @todo: draw enum flag UI
        //public Modifier[] modifiers;
    }

    public struct ItemInstance
    {
        public ItemTemplate Template;
        public int Amount;

        public ItemInstance (ItemTemplate template, int amount)
        {
            Template = template;
            Amount = amount;
        }

        public bool IsEmpty ()
        {
            return Template != null || Amount < 1;
        }

        public void Clear ()
        {
            Template = null;
            Amount = 0;
        }
    }

}