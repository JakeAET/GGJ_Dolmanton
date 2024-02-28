using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject lrObj;
    private LineRenderer lr;
    [SerializeField] Transform target;
    private Vector3 start;

    // Start is called before the first frame update
    void Start()
    {
        lr = lrObj.GetComponent<LineRenderer>();
        start = new Vector3(target.localPosition.x, target.localPosition.y, 0f);
        lr.SetPosition(0, start);
    }

    // Update is called once per frame
    void Update()
    {
        if (lrObj.activeInHierarchy)
        {
            lr.SetPosition(1, new Vector3(target.localPosition.x, target.localPosition.y, 0f));
        }
        else
        {
            lr.SetPosition(1, start);
        }
    }
}
