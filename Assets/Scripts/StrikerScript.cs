using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikerScript : MonoBehaviour
{
    public float PowerAdjuster = 1f;
    public float MaxPower = 2f;
    public float MinDistance = 1f;
    public float MinMagnitudeVelocity = 0.4f;
    public Rigidbody2D Rb;
    public GameObject PowerCircle;
    public GameObject Arrow;
    public GameObject GhostStiker;
    public GameObject MoverStriker;
    public GameObject Mover;
    public GameObject PointA;
    public GameObject PointB;
    public LayerMask LayersToHitCoins;
    public LayerMask LayersToHitEdges;
    public LayerMask LayersToHitPlayer;
    public CircleCollider2D Collider;
    public LineRenderer AimingLine;

    
    public static bool StopMoverStiker = false;
    public static bool StrikerFired = false;
    public static bool CollisionDetected = false;
    public static bool MakeStrikerCollide = true;
    Vector3 TouchPos, DragPos, ReleasePos;
    Vector3 StartPosStriker, StartPosMover;
    float PowerToShoot;
    bool CancelAttack = false, FiringPos=false;
    SpriteRenderer StrikerSprite,MoverSprite;

    private void Awake()
    {
        StartPosStriker = transform.position;
        StartPosMover = MoverStriker.transform.position;
        StrikerSprite = gameObject.GetComponent<SpriteRenderer>();
        MoverSprite = Mover.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        TouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }
    private void OnMouseDrag()
    {
        if (!StrikerFired && !CollisionDetected && !MainMenuScript.TimeUp)
        {
            MoverSprite.color = Color.gray;  
            FiringPos = false;
            Rb.freezeRotation = false;      // Allowing to rotate
            Collider.isTrigger = false;     // Disable trigger
            CoinsScript.StopCoins = false;  // Allowing Coins To move
            DragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PowerToShoot = Vector3.Distance(TouchPos, DragPos);
            if (PowerToShoot < MinDistance)
            {
                CancelAttack = true;
                FiringPos = true;          // Enabling Tigger
                DeActivateStuff();
            }
            else
            {
                CancelAttack = false;
                PowerToShoot = Mathf.Clamp(PowerToShoot, MinDistance, MaxPower);
                ResizingStuff();
                PredictingHitDirection();
            }
        }
        else
        {
            DeActivateStuff();
        }
        
    }
    private void OnMouseUp()
    {
        if (!CancelAttack && !StrikerFired && !CollisionDetected && !MainMenuScript.TimeUp)
        {
            DeActivateStuff();
            ReleasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 DirectionOfForce = TouchPos - ReleasePos;
            Rb.AddForce(DirectionOfForce * PowerToShoot *PowerAdjuster , ForceMode2D.Impulse);
            StrikerFired = true;
            StopMoverStiker = true;              //Stopping from adjusting position of Striker
        }
        MoverSprite.color = Color.white;
    }


    private void Update()
    {
        if (StrikerFired)
        {
            if (Rb.velocity.magnitude < MinMagnitudeVelocity)
            {
                
                Rb.velocity = Vector2.zero;
                Rb.freezeRotation = true;        // Freezing Rotation
                StrikerFired = false;
                StartCoroutine(StuffToDoAfterStrikerIsFired());
              
            }
            else
            {
                MoverSprite.color = Color.gray;
            }
        }
        if (FiringPos)
        {
            RaycastHit2D Ray = Physics2D.Linecast(PointA.transform.position, PointB.transform.position, LayersToHitPlayer);
            Debug.DrawLine(PointA.transform.position, PointB.transform.position, Color.red);
            if (Ray.collider != null)
            {
                Collider.isTrigger = true;
            }
        }
        else
        {
            if (!MakeStrikerCollide)
            {
                Collider.isTrigger = true;
            }
            else
            {
                Collider.isTrigger = false;
            }
        }
    }
    
 
    IEnumerator StuffToDoAfterStrikerIsFired()
    {
        yield return new WaitForSeconds(2);
        StrikerSprite.color = Color.white;  // If striker got into one of the holes
        MoverSprite.color = Color.white;
        transform.position = StartPosStriker;
        
        MoverStriker.transform.position = StartPosMover;
        FiringPos = true;
        StopMoverStiker = false;         //Allowing To Adjust Pos of Striker
        Collider.isTrigger = true;   // Striker doesn't collide with anything at the original position of striker
        Rb.velocity = Vector2.zero;

        CoinsScript.StopCoins = true;   // Stoping the position of all coins after the Striker return to striking field


        if (!HoleScript.PlayerScored)
        {
            MainMenuScript.ChangePlayer(); // Changing Players

        }
        HoleScript.PlayerScored = false;  // If player missed after scoring once 

        MainMenuScript MMS = GameObject.FindObjectOfType<MainMenuScript>();     //Refilling the time bar
        MMS.RefillTime();

    }
       
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Red"|| collision.tag == "Black"|| collision.tag == "White")
        {
            CollisionDetected = true;
            StrikerSprite.color = Color.red;
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Red" || collision.tag == "Black" || collision.tag == "White")
        {
            CollisionDetected = false;
            StrikerSprite.color = Color.white;
        }
    }

    
    void PredictingHitDirection()
    {
        RaycastHit2D Hit = Physics2D.CircleCast(transform.position, 0.15f, Arrow.transform.up, Mathf.Infinity,LayersToHitCoins);
        if (Hit.collider != null)
        {
            AimingLine.enabled = true;
            GhostStiker.SetActive(true);
            GhostStiker.transform.position = Hit.centroid;
           // Debug.DrawLine(transform.position, Hit.centroid,Color.red);
            Vector2 dd = new Vector2(Hit.transform.position.x,Hit.transform.position.y) - Hit.centroid;
           // Debug.DrawRay(Hit.centroid, dd, Color.yellow);
            AimingLine.SetPosition(0, transform.position);
            AimingLine.SetPosition(1, Hit.centroid);

            RaycastHit2D targetDir = Physics2D.Raycast(Hit.point, dd, Mathf.Infinity, LayersToHitEdges);

            AimingLine.SetPosition(2, targetDir.point);
           
        }
        else
        {
            RaycastHit2D HitEdges = Physics2D.CircleCast(transform.position, 0.15f, Arrow.transform.up, Mathf.Infinity, LayersToHitEdges);
            if (HitEdges.collider != null)
            {
                AimingLine.enabled = true;
                GhostStiker.SetActive(true);
                GhostStiker.transform.position = HitEdges.centroid;
               // Debug.DrawLine(transform.position, HitEdges.centroid, Color.red);
                Vector3 ReflectDir = Vector3.Reflect((new Vector3(HitEdges.centroid.x, HitEdges.centroid.y, 0) - Arrow.transform.position).normalized, HitEdges.normal);
               // Debug.DrawRay(HitEdges.centroid, ReflectDir, Color.white);
                AimingLine.SetPosition(0, transform.position);
                AimingLine.SetPosition(1, HitEdges.centroid);

                RaycastHit2D targetDirEd = Physics2D.Raycast(HitEdges.centroid, ReflectDir, Mathf.Infinity, LayersToHitEdges);

                AimingLine.SetPosition(2, targetDirEd.point);
            }
        }
    }

    void ResizingStuff()
    {
        PowerCircle.SetActive(true);
       // Arrow.SetActive(true);
        Arrow.transform.rotation = Quaternion.LookRotation(Vector3.forward,transform.position-DragPos);

        
      //  Arrow.transform.localScale = new Vector3(PowerToShoot / MaxPower, PowerToShoot / MaxPower, PowerToShoot / MaxPower);
        PowerCircle.transform.localScale = new Vector3(PowerToShoot / MaxPower, PowerToShoot / MaxPower, PowerToShoot / MaxPower);
    }

    public void DeActivateStuff()
    {
        PowerCircle.SetActive(false);
     //   Arrow.SetActive(false);
        GhostStiker.SetActive(false);
        AimingLine.enabled = false;
    }
}
