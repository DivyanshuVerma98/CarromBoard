using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    void AnimationComplete()
    {
        StrikerScript.CollisionDetected = false;   // So player can now shoot  after board rotation is complete
        StrikerScript.StopMoverStiker = false;    // So player can now change position of striker
        StrikerScript.MakeStrikerCollide = true;  // So stiker can collide
    }
}
