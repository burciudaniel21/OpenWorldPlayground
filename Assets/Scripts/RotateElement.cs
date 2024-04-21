using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateElement : MonoBehaviour
{
    public enum RotationAxis
    {
        X,
        Y,
        Z
    }

    public RotationAxis rotationAxis = RotationAxis.Y;
    public float rotationSpeed = 50f;

    void Update()
    {
        // Determine the rotation axis based on the selected enum value
        Vector3 axisVector = Vector3.zero;
        switch (rotationAxis)
        {
            case RotationAxis.X:
                axisVector = Vector3.right;
                break;
            case RotationAxis.Y:
                axisVector = Vector3.up;
                break;
            case RotationAxis.Z:
                axisVector = Vector3.forward;
                break;
        }

        // Rotate the object around the selected axis
        transform.Rotate(axisVector * rotationSpeed * Time.deltaTime);
    }
}
