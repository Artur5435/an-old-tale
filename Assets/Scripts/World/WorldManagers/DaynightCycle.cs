using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaynightCycle : MonoBehaviour
{

    public float speed;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(speed * Time.deltaTime, 0, 0);
    }
}
