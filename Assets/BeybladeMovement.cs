using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeybladeMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float rotationSpeed = 1f;

    public float xAngle = 0f, yAngle = 10.0f, zAngle = 0f;
    public float xConForce, yConForce = -5, zConForce;

    public float conForceUpper = 50;
    public float conForceLower = -50;
    // Update is called once per frame
    void Update()
    {
        xConForce = Random.Range(conForceLower, conForceUpper);
        zConForce = Random.Range(conForceLower, conForceUpper);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        GetComponent<ConstantForce>().force = new Vector3(xConForce, yConForce, zConForce);
        transform.Rotate(xAngle, yAngle, xAngle, Space.Self);
    }
}
