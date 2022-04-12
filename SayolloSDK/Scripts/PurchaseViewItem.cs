using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PurchaseViewItem
{
        public string title;
        public string itemName;
        public string itemImageLink;
        public string price;
        public string currency;
        public string currencySign;

        public PurchaseViewItem(string title, string itemName, string itemImageLink, string price, string currency, string currencySign)
        {
            this.title = title;
            this.itemName = itemName;
            this.itemImageLink = itemImageLink;
            this.price = price;
            this.currency = currency;
            this.currencySign = currencySign;
        }
    }
