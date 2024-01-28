using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] GameObject line;
    [SerializeField] GameObject arrow;

    private LineRenderer lr;

    [SerializeField] float maxDistance;
    [SerializeField] float maxForce;

    private Vector3 lineStart;
    private Vector3 mouseStart;
    private float distance;

    private bool drawingLine;


    // Start is called before the first frame update

    void Awake()
    {
        lr = line.GetComponent<LineRenderer>();
        line.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.allowLaunch)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!drawingLine)
                {
                    lineStart = GameManager.instance.activePlayer.slingshotPoint.position;
                    mouseStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    line.SetActive(true);
                    arrow.SetActive(true);
                    drawingLine = true;
                }
            }

            if (Input.GetMouseButton(0) && line.activeInHierarchy)
            {
                lineStart = GameManager.instance.activePlayer.slingshotPoint.position;
                GameManager.instance.activePlayer.switchFace(false);

                Vector3 mousePosition = Input.mousePosition;
                distance = (lineEnd() - lineStart).magnitude;

                Vector3 arrowDirection = (lineStart - lineEnd()).normalized;
                arrow.transform.position = new Vector3(lineStart.x, lineStart.y, 0f);
                float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                lr.SetPosition(0, new Vector3(lineStart.x, lineStart.y, 0f));
                lr.SetPosition(1, new Vector3(lineEnd().x, lineEnd().y, 0f));
            }

            if (Input.GetMouseButtonUp(0) && drawingLine)
            {
                launchBall();
                lineStart = Vector2.zero;
                line.SetActive(false);
                arrow.SetActive(false);
                drawingLine = false;
            }
        }
    }

    Vector3 lineEnd()
    {
        Vector2 mouseEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float xDiff = mouseEnd.x - mouseStart.x;
        float yDiff = mouseEnd.y - mouseStart.y;

        Vector3 newPos = new Vector3(lineStart.x + xDiff, lineStart.y + yDiff, 0);
        Vector3 offset = newPos - lineStart;

        return lineStart + Vector3.ClampMagnitude(offset, maxDistance);
    }

    void launchBall()
    {
        Vector3 launchDirection = (lineStart - lineEnd()).normalized;
        float appliedForce = Mathf.Lerp(0, maxForce, Mathf.InverseLerp(0, maxDistance, distance));
        //Debug.Log("Force: " + appliedForce + "  current distance: " + distance);
        GameManager.instance.activePlayer.golfRB.AddForce(launchDirection * appliedForce, ForceMode2D.Impulse);
        GameManager.instance.allowLaunch = false;
        GameManager.instance.launchFinished = true;
    }
}
