using System;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private float speed = 5f;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer==LayerMask.NameToLayer("Ground"))
        {
            rigidbody2D.AddForce(col.transform.right*speed,ForceMode2D.Impulse); //this right will be red arrows on Scenes and with forcemode2d.impuls its will be instant force
            
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("BarrelDestroyer"))
        {
            Destroy(this.gameObject); //when the barrels comes end of the scene they will destroy
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            //TO DO Destroy barrels and add 500 point
        }
    }
}
