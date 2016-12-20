using UnityEngine;
using System.Collections;

public class PlayerSight : MonoBehaviour {
    float distance = 10.0f;
    float lookTimer = 0.0f;
    float lookTime = 1.5f;
    public UnitychanAnimationControll unitychanAnimationControll;
    public LayerMask characterLayer;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 forwardDirection = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position,forwardDirection,distance,characterLayer))
        {
            lookTimer += Time.deltaTime;
            if (lookTimer >= lookTime)
            {
                unitychanAnimationControll.IsLooked = true;
                lookTimer = 0.0f;
            }
        }
        else
        {
            unitychanAnimationControll.IsLooked = false;
        }
	}
}
