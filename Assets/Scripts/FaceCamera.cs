using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Update is called once per frame
    Vector3 pos;
    private void Start()
    {
        pos = this.gameObject.transform.position;
    }
    void Update()
    {
        this.gameObject.transform.position = pos;
    }
}
