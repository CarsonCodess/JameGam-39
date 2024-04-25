using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoseHeal : MonoBehaviour
{
    void Start()
    {
        
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            Player.instance.Heal();
            Destroy(gameObject);
        }
    }
}
