using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public Item Item { get; private set; }
    public void PickUp(Item newItem)
    {
        if (Item != null)
            DropItem(newItem.transform.position);

        this.Item = newItem;
        this.Item.ItemPickedUp();
    }
    private void DropItem(Vector3 dropLocation)
    {
        Item.ItemDropped(dropLocation);
    }

    public void HandInItem(Grave graveQuestGiver)
    {
        graveQuestGiver.HandInItem(Item);
        Item = null;
    }
}
