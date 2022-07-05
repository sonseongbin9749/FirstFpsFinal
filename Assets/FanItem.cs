using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanItem : MonoBehaviour, UnknownItem
{
    public void UnknownItemsystem(int unknownitemcount)
    {
        ItemText.scoreValue += unknownitemcount;

    }
}
