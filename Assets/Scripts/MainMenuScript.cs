using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Slider SliderA;
    public Slider SliderB;
    public Text ScoreA;
    public Text ScoreB;
    public  GameObject PlayerAText;
    public  GameObject PlayerBText;
    public Animator BoardAnim;

    public static GameObject Board;
    public static bool TimeUp = false;
    public static bool PlayerATurn = false, PlayerBTurn = false;
    public static int PlayerAScore = 0, PlayerBScore = 0;
    public static bool TurnChange=false;

    private void Awake()
    {
        Board = GameObject.Find("Board");
        SliderA.value = SliderA.maxValue;      // To increase time for just increase the max value if slider 
        SliderB.value = SliderB.maxValue;       // To increase time for just increase the max value if slider
        PlayerATurn = true;
        PlayerBScore = PlayerAScore = 0;
        
    }
    void Update()
    {
        if (PlayerATurn)
        {
            ScoreA.text = PlayerAScore.ToString();
            if (!StrikerScript.StrikerFired)
                SliderA.value -= Time.deltaTime;

            if (SliderA.value <= 0)
            {
                TimeUp = true;
                ChangePlayer();
                RefillTime();
            }
           
        }
        if (PlayerBTurn)
        {
            ScoreB.text = PlayerBScore.ToString();
            if (!StrikerScript.StrikerFired)
                SliderB.value -= Time.deltaTime;

            if (SliderB.value <= 0)
            {
                TimeUp = true;
                ChangePlayer();
                RefillTime();
            }
        }

        if (TurnChange)
        {
            if (PlayerATurn)
            {
                PlayerAText.SetActive(true);
                BoardAnim.SetTrigger("ReverseBoard");
                

            }
            if (PlayerBTurn)
            {
                PlayerBText.SetActive(true);
                BoardAnim.SetTrigger("RotateBoard");

            }
            TurnChange = false;
        }
        
    }

    public static void ChangePlayer()
    {
        PlayerATurn = !PlayerATurn;
        PlayerBTurn = !PlayerBTurn;
        TimeUp = false;
        TurnChange = true;
        StrikerScript.CollisionDetected = true;   // So player can't fire while board is rotating
        StrikerScript.StopMoverStiker = true;   // So player can't move the position of striker while the board is rotating
        StrikerScript.MakeStrikerCollide = false;  // So striker doesnt collide with other tokken while rotating
    }

    

    public void RefillTime()
    {
        SliderA.value = SliderA.maxValue;
        SliderB.value = SliderB.maxValue;

    }
}
