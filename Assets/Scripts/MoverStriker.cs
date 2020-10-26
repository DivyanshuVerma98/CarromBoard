using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverStriker : MonoBehaviour
{
    public GameObject Striker;
    public GameObject Mover;
    bool isDragging;
    Vector3 MouseStrikerPos,StrikerPos;
    SpriteRenderer MoverSprite;

    private void Awake()
    {
        MoverSprite = Mover.GetComponent<SpriteRenderer>();
        
    }
    private void OnMouseDown()
    {
        isDragging = true;
        MoverSprite.color = Color.gray;
    }
    private void OnMouseUp()
    {
        isDragging = false;
        MoverSprite.color = Color.white;
    }

    private void Update()
    {
        if (isDragging && !StrikerScript.StopMoverStiker && !MainMenuScript.TimeUp)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            MouseStrikerPos = transform.position;
            MouseStrikerPos.x = worldPos.x;
            MouseStrikerPos.x = Mathf.Clamp(MouseStrikerPos.x,-1.4f, 1.4f);
            transform.position = MouseStrikerPos;

            StrikerPos = Striker.transform.position;
            StrikerPos.x = worldPos.x;
            StrikerPos.x = Mathf.Clamp(StrikerPos.x, -1.2f, 1.2f);
            Striker.transform.position = StrikerPos;

        }
       
    }
}
