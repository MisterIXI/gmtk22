using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPositions : MonoBehaviour
{
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.position;
    }
}
