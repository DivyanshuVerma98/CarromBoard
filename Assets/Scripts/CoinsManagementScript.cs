using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsManagementScript : MonoBehaviour
{
    public bool AllCoinsStopped = false;
    Rigidbody2D[] CoinsRb;
        
    private void Update()
    {
        int NoOfCoins = transform.childCount;
        int i = 0;
        
        for(i = 0; i < NoOfCoins; i++)
        {
            GameObject coin = transform.GetChild(i).gameObject;
            Rigidbody2D Rb = coin.GetComponent<Rigidbody2D>();
            if (Rb.velocity.magnitude < 0.1f)
                AllCoinsStopped = true;
            else
            {
                AllCoinsStopped = false;
                Debug.Log(i + " " + coin.name);
                CoinIsMoving(Rb);
            }

        }
    }

    void CoinIsMoving(Rigidbody2D CoinMovingRb)
    {
        if (CoinMovingRb.velocity.magnitude < 0.1f)
        {
            AllCoinsStopped = true;
            CoinMovingRb.velocity = Vector2.zero;
        }
        else
        {
            AllCoinsStopped = false;

        }
    }

}
