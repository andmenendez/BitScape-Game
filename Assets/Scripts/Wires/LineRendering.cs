using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendering : MonoBehaviour
{

    private LineRenderer lr;
    //public List<GameObject>[] joints;
    public List<GameObject> joints;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = joints.Count;
        updateLine();
    }

    // Update is called once per frame
    void Update()
    {
        updateLine();
    }

    void updateLine()
    {
        for (int i = 0; i < joints.Count; i++)
        {
            lr.SetPosition(i, joints[i].transform.position);
        }
    }
}
