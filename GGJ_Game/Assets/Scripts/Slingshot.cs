using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] Color highPowerColor;
    [SerializeField] Color midPowerColor;
    [SerializeField] Color lowPowerColor;

    [SerializeField] GameObject line; // Standalone gameobject that contains the LineRenderer component
    [SerializeField] GameObject arrow; // Arrow sprite gameobject

    private LineRenderer lr;

    [SerializeField] float maxDistance; // Maximum distance the line renderer can be pulled to
    [SerializeField] float maxForce; // Maximum force achieved by maximum pull-back distance

    private Vector3 lineStart;
    private Vector3 mouseStart;
    public Vector2 mouseStartDebug;
    public Vector2 mouseEndDebug;
    public float distance; // distance from the line start point to the line end point
    public float actualDistance;

    private bool drawingLine; // true if line is being drawn

    [SerializeField] CamController camContRef;
    [SerializeField] float maxCamZoomOut;
    [SerializeField] float minCamZoomOut;

    //private Camera touchCam;

    void Awake()
    {
        // Assign line renderer component and turn off the line until it's is needed
        lr = line.GetComponent<LineRenderer>();
        line.SetActive(false);
    }

    void Start()
    {
        //touchCam = GameObject.FindGameObjectWithTag("TouchCam").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.allowLaunch) // Checks the GameManager script if it's a player's turn to launch
        {
            if (Input.GetMouseButtonDown(0)) // Calls when the left mouse button is first pressed down
            {
                if (!drawingLine) // Only runs at the start of the line draw, then sets bool to false
                {
                    lineStart = GameManager.instance.activePlayer.slingshotPoint.position; // Assign line start position
                    //mouseStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Assign mouse start position
                    mouseStart = new Vector2(50 * (Input.mousePosition.x / Screen.width), 50 * (Input.mousePosition.y / Screen.height));
                    //mouseStart = touchCam.ScreenToWorldPoint(Input.mousePosition);
                    mouseStartDebug = mouseStart;
                    line.SetActive(true);
                    arrow.SetActive(true);
                    drawingLine = true;
                    if (GameManager.instance.tutorialObj.activeInHierarchy)
                    {
                        GameManager.instance.tutorialObj.SetActive(false);
                    }
                }
            }

            if (Input.GetMouseButton(0) && line.activeInHierarchy)
            {
                lineStart = GameManager.instance.activePlayer.slingshotPoint.position;
                GameManager.instance.activePlayer.switchFace(false); // Player face forward when launch starts

                distance = (lineEnd() - lineStart).magnitude; // Calculate distance from line start to line end

                if (distance >= 1)
                {
                    //Debug.Log("slingshot changed size: " + (maxCamZoomOut + (minCamZoomOut - maxCamZoomOut) * (distance / maxDistance)));
                    //camContRef.zoomInZoomOut(Mathf.Round(maxCamZoomOut + (minCamZoomOut - maxCamZoomOut) * (distance / maxDistance)), 0.1f);w
                    camContRef.zoomInZoomOut(maxCamZoomOut + (minCamZoomOut - maxCamZoomOut) * (distance / maxDistance), 0.1f);
                }

                Vector3 arrowDirection = (lineStart - lineEnd()).normalized; // Calculate direction arrow sprite should face
                arrow.transform.position = new Vector3(lineStart.x, lineStart.y, 0f); // Lock arrow position to line start
                float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Rotate arrow to target angle

                // Update line renderer start and end positions
                lr.SetPosition(0, new Vector3(lineStart.x, lineStart.y, 0f));
                lr.SetPosition(1, new Vector3(lineEnd().x, lineEnd().y, 0f));
                Color startColor = lineColor();
                Color endColor = lineColor();
                startColor.a = 0.7f;
                endColor.a = 0.3f;
                lr.startColor = startColor;
                lr.endColor = endColor;
            }

            // Check if mouse button is released, line is being drawn, and the distance is large enough
            if (Input.GetMouseButtonUp(0) && drawingLine && distance >= 0.5f)
            {
                launchBall();
                camContRef.zoomInZoomOut(15, 2f);

                // Reset values
                lineStart = Vector2.zero;
                line.SetActive(false);
                arrow.SetActive(false);
                drawingLine = false;
            }
            else if(Input.GetMouseButtonUp(0) && drawingLine)
            {
                // Reset Values without launching, distance was too small
                lineStart = Vector2.zero;
                line.SetActive(false);
                arrow.SetActive(false);
                drawingLine = false;
            }
        }
    }

    // Returns calculated line end postion
    Vector3 lineEnd()
    {
        //Vector2 mouseEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseEnd = new Vector2(50 * (Input.mousePosition.x / Screen.width), 50 * (Input.mousePosition.y / Screen.height));
        //Vector2 mouseEnd = touchCam.ScreenToWorldPoint(Input.mousePosition);
        mouseEndDebug = mouseEnd;

        // Determine x and y difference from mouse start to end points
        float xDiff = mouseEnd.x - mouseStart.x;
        float yDiff = mouseEnd.y - mouseStart.y;

        Vector3 newPos = new Vector3(lineStart.x + xDiff, lineStart.y + yDiff, 0);
        Vector3 offset = newPos - lineStart;

        actualDistance = ((lineStart + offset) - lineStart).magnitude;

        // Find line end by applying the same offset from mouse points to the line points
        return lineStart + Vector3.ClampMagnitude(offset, maxDistance);
    }

    void launchBall()
    {
        // Play sound effect
        AudioManager.instance.PlayCustomPitch("bink", Random.Range(0.6f, 1.4f));

        if(GameManager.instance.activePlayer.golfRB.constraints == RigidbodyConstraints2D.FreezeAll)
        {
            GameManager.instance.activePlayer.golfRB.constraints = RigidbodyConstraints2D.None;
        }

        // Calculate direction of launch
        Vector3 launchDirection = (lineStart - lineEnd()).normalized;

        // Convert the distance of the mouse drag to the amount of force applied to the ball
        float appliedForce = Mathf.Lerp(0, maxForce, Mathf.InverseLerp(0, maxDistance, distance));
        //Debug.Log("Force: " + appliedForce + "  current distance: " + distance);

        // Apply the force to the target rigid body and set force mode to instant force applied
        GameManager.instance.activePlayer.golfRB.AddForce(launchDirection * appliedForce, ForceMode2D.Impulse);
        GameManager.instance.allowLaunch = false;
        GameManager.instance.launchFinished = true;
    }

    private Color lineColor()
    {
        if(distance > maxDistance * 0.6f) // mid to high power shot
        {
            float newR = Mathf.Lerp(midPowerColor.r, highPowerColor.r, Mathf.InverseLerp(maxDistance * 0.6f, maxDistance, distance));
            float newG = Mathf.Lerp(midPowerColor.g, highPowerColor.g, Mathf.InverseLerp(maxDistance * 0.6f, maxDistance, distance));
            float newB = Mathf.Lerp(midPowerColor.b, highPowerColor.b, Mathf.InverseLerp(maxDistance * 0.6f, maxDistance, distance));
            return new Color(newR, newG, newB);
        }
        else if (distance > maxDistance * 0.3f)// low to mid power shot
        {
            float newR = Mathf.Lerp(lowPowerColor.r, midPowerColor.r, Mathf.InverseLerp(maxDistance * 0.3f, maxDistance * 0.6f, distance));
            float newG = Mathf.Lerp(lowPowerColor.g, midPowerColor.g, Mathf.InverseLerp(maxDistance * 0.3f, maxDistance * 0.6f, distance));
            float newB = Mathf.Lerp(lowPowerColor.b, midPowerColor.b, Mathf.InverseLerp(maxDistance * 0.3f, maxDistance * 0.6f, distance));
            return new Color(newR, newG, newB);
        }
        else
        {
            return lowPowerColor;
        }
    }

    private void FixedUpdate()
    {
        //Color color = new Color(1, 0, 0);
        //Vector3 startDebugLine = new Vector3(mouseStartDebug.x, mouseStartDebug.y, 1);
        //Vector3 endDebugLine = new Vector3(mouseEndDebug.x, mouseEndDebug.y, 1);
        //Debug.DrawLine(startDebugLine, endDebugLine, color);
    }
}
