using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSmasher : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer==LayerMask.NameToLayer("Barrel"))
        {
            //col.gameObject.SetActive(false);
            Destroy(col.gameObject);
            
        }
    }
}
