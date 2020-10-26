using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsScript : MonoBehaviour
{
    public static bool StopCoins=false;
    public Rigidbody2D Rb;
    private void Update()
    {        
        if (StopCoins)
        {
            Rb.velocity = Vector2.zero;
            
        }

    }
}
