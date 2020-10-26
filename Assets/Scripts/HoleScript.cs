using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour
{
    int Point = 0;
    public static bool PlayerScored = false;
    public GameObject BlackCoin;
    public GameObject BoardCenterLocation;
    public GameObject CoinsManager;

    GameObject Striker;
    SpriteRenderer StrikerSprite;
    CircleCollider2D StrikerCollider;
    Rigidbody2D StrikerRb;
    private void Awake()
    {
        Striker = GameObject.FindGameObjectWithTag("Striker");
        StrikerSprite = Striker.GetComponent<SpriteRenderer>();
        StrikerCollider = Striker.GetComponent<CircleCollider2D>();
        StrikerRb = Striker.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Black":
                Point = 10;
                Destroy(collision.gameObject);
                PlayerScored = true;
                break;
            case "White":
                Point = 20;
                Destroy(collision.gameObject);
                PlayerScored = true; 
                break;
            case "Red":
                Point = 50;
                Destroy(collision.gameObject);
                PlayerScored = true;
                break;
            case "Striker":
                Point = -10;
                PlayerScored = false;
                StrikerCollider.isTrigger = true;
                StrikerSprite.color = Color.clear;
                StrikerRb.velocity = Vector2.zero;
                StrikerInHole();
                break;
            
        }
        
        if (MainMenuScript.PlayerATurn)
        {
            MainMenuScript.PlayerAScore += Point;
        }
        if (MainMenuScript.PlayerBTurn)
        {
            MainMenuScript.PlayerBScore += Point;
        }
    }

    void StrikerInHole()
    {
        if (MainMenuScript.PlayerATurn)
        {
            if (MainMenuScript.PlayerAScore > 0)
            {
                GameObject Coin= Instantiate(BlackCoin, BoardCenterLocation.transform.position, Quaternion.identity);
                Coin.transform.SetParent(CoinsManager.transform);
            }
        }

        if (MainMenuScript.PlayerBTurn)
        {
            if (MainMenuScript.PlayerBScore > 0)
            {
                GameObject Coin=Instantiate(BlackCoin, BoardCenterLocation.transform.position, Quaternion.identity);
                Coin.transform.SetParent(CoinsManager.transform);
            }
        }
    }
    
}
