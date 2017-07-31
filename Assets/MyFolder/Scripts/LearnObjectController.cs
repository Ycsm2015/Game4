using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LearnObjectController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p0 = Vector3.zero;

        Vector3 p1 = new Vector3(-1.0f, 1.0f, 0.0f);
        Vector3 p2 = new Vector3(1f, 2f, 0.0f);

        Debug.DrawLine(p0, p1, Color.black);
        Debug.DrawLine(p0, p2, Color.black);

        Debug.DrawLine(p1, p2, Color.red);

        Vector3 center = (p1 + p2) * 0.5f;

        Debug.DrawLine(p0, center, Color.black);

        Vector3 centerProject = Vector3.Project(center, p1 - p2);

        Debug.DrawLine(p0, centerProject, Color.black);
        Debug.DrawLine(center, centerProject, Color.green);
        {
            Vector3 centerTmp = center - centerProject;
            Debug.DrawLine(center, centerTmp, new Color(1f,1f,0));
        }
        center = Vector3.MoveTowards(center, centerProject, (center- centerProject).magnitude);
        Debug.DrawLine(Vector3.zero, center, Color.cyan);
        
        //center -= new Vector3(0f, 1f, 0f);
        p1 -= center;
        p2 -= center;
        for (int i = 1; i <= 20; i++)
        {
            Vector3 to = Vector3.Slerp(p1, p2, 0.05f * i);
            //画出直线，以便观察
            Debug.DrawLine(center, to + center, Color.yellow);
        }

        Vector3 v1 = new Vector3(1, -1, 0);
        Vector3 v2 = new Vector3(1, 2, 0);
        Vector3 v3 = Vector3.Project(v1, v2);

        //Debug.DrawLine(Vector3.zero, v1, Color.blue);
        //Debug.DrawLine(Vector3.zero, v2, Color.green);
        // Debug.DrawLine(Vector3.zero, v3, Color.red);
    }
}
