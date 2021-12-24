using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerDestroyer : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D col)
   {
      if (col.gameObject.CompareTag("Player"))
      {
         Destroy(this.gameObject);
      }
   }
}
