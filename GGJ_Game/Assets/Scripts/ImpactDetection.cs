using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ImpactDetection : MonoBehaviour
{
    private List<Vector2> impactPoints = new List<Vector2>();
    [SerializeField] GameObject impactEffect;

    CamController camCont;

    // Start is called before the first frame update
    void Start()
    {
        camCont = FindObjectOfType<CamController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // small hit: 0 - 15
        // medium hit: 15 - 30
        // big hit: 30+

        //Debug.Log("Collision 2D detected from " + col.collider.name + ". Magnitude: " + col.relativeVelocity.magnitude);

        // Big Hit
        if (col.relativeVelocity.magnitude >= 5)
        {
            foreach (ContactPoint2D impact in col.contacts)
            {
                Vector2 impactPoint = impact.point;

                bool validImpact = true;

                if (impactPoints.Count > 0)
                {
                    foreach (Vector2 p in impactPoints)
                    {
                        if (Vector2.Distance(impactPoint, p) <= 1)
                        {
                            //Debug.Log("Duplicate point found");
                            validImpact = false;
                            break;
                        }
                    }
                }

                if (validImpact)
                {
                    impactPoints.Add(impactPoint);

                    Debug.Log(col.collider.name + " impact point at " + impactPoint.x + ", " + impactPoint.y + " with a magnitude of " + col.relativeVelocity.magnitude);
                    //Debug.Log(col.collider.transform.parent.gameObject.name);

                    // calculate angle

                    Vector2 objectCenter = col.transform.position; // Get object center

                    Vector2 directionVector = impactPoint - objectCenter; // Calculate direction vector

                    float collisionAngle = Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg; // Get angle

                    //Debug.Log(collisionAngle);

                    // spawn impact effect

                    //newEffect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, collisionAngle));

                    if(col.relativeVelocity.magnitude >= 30)
                    {
                        //camCont.startCameraShake(col.collider.transform.parent.gameObject.name, 16f, 0.5f);
                        GameObject newEffect = Instantiate(impactEffect, new Vector3(impactPoint.x, impactPoint.y, 0), Quaternion.Euler(new Vector3(collisionAngle, -90, -90)));
                    }
                    else if(col.relativeVelocity.magnitude >= 20)
                    {
                        //camCont.startCameraShake(col.collider.transform.parent.gameObject.name, 8f, 0.5f);
                    }
                    else if (col.relativeVelocity.magnitude >= 10)
                    {
                        //camCont.startCameraShake(col.collider.transform.parent.gameObject.name, 4f, 0.5f);
                    }
                    else
                    {
                        //camCont.startCameraShake(col.collider.transform.parent.gameObject.name, 2f, 0.5f);
                    }

                    StartCoroutine(collisionMonitor(impactPoint));
                }
            }
        }
        //else
        //{
        //    camCont.startCameraShake(col.collider.transform.parent.gameObject.name, 4f, 0.2f);
        //}


        //else if (col.relativeVelocity.magnitude >= 30)
        //{
        //    Debug.Log("Check");
        //    camCont.startCameraShake(col.collider.transform.parent.gameObject.name, 1f, 0.2f);
        //}
    }

    IEnumerator collisionMonitor(Vector2 point)
    {
        //Debug.Log(point);

        yield return new WaitForSeconds(0.2f);

        impactPoints.Remove(point);

        yield return null;
    }
}
