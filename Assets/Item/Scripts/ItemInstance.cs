using System;
using System.Collections;
using UnityEngine;

namespace uItem
{
    public class ItemInstance<T> where T : ItemTemplate, new ()
    {
        public T Template;
        public int Amount;

        public ItemInstance () { }

        public ItemInstance (T template, int amount)
        {
            Template = template;
            Amount = amount;
        }

        public bool IsEmpty ()
        {
            return Template == null || Amount < 1;
        }
    }
}