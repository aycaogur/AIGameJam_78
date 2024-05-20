using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamonds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Inventory inventory=other.GetComponent<Inventory>();

        if(inventory!=null )
        {
            inventory.DiamondCollected();
            gameObject.SetActive(false);
        }
    }
}
