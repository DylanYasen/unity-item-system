using System;
using System.Collections;
using UnityEngine;

namespace uItem
{
    public class ItemInstance
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
            return Template == null || Amount < 1;
        }

        public void Clear ()
        {
            Template = null;
            Amount = 0;
        }
    }
}