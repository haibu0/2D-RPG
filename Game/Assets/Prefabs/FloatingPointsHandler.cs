using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPointsHandler : MonoBehaviour
{
    void Start()
    {

        transform.localPosition += new Vector3(0, .1f, 0);
        if (gameObject.name == "FloatingParent(Clone)")
        {
            Destroy(gameObject, 1f);
        }
    }
}
