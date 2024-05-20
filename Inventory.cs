using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public int DiamondsNumber { get;private set; }
    public UnityEvent<Inventory> OnDiamondCollected;

    public void DiamondCollected()
    {
        DiamondsNumber++;
        OnDiamondCollected.Invoke(this);
    }
}
