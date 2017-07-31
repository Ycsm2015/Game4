using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float ZoomSpeed;
    public float currentZoom;

	public float minZoom = 0;
    public float maxZoom = 3;

    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;

    public float MinimumX = -90f;
    public float MaximumX = 0f;

    public Transform CameraCenterPoint;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        modifyCamera();
        TurningCamera();
    }

    void modifyCamera()
    {
        float wheelState = Input.GetAxis("Mouse ScrollWheel");
        if (wheelState != 0)
        {
            float increment = wheelState * ZoomSpeed;

            if (wheelState > 0)
            {
                //ZoomIn

                if (currentZoom >= maxZoom) return;
            }
            else
            {
                //ZoomOut
                if (currentZoom <= minZoom)
                {
                    enterTrueLifeMode();
                    return;
                }
            }
			increment = Mathf.Clamp(increment, minZoom - currentZoom, maxZoom - currentZoom);
			transform.position += transform.forward * increment;
			currentZoom += increment;
        }
    }

    void enterTrueLifeMode()
    {

    }

    void TurningCamera()
    {
        bool isScrollWheelKeyDown = Input.GetKey(KeyCode.Mouse2);
        if (isScrollWheelKeyDown)
        {
            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            CameraCenterPoint.Rotate(xRot, 0, 0, Space.Self);
            CameraCenterPoint.Rotate(0, yRot, 0, Space.World);

            //if (clampVerticalRotation)
            //{
            //    CameraCenterPoint.localRotation = 
            //    ClampRotationAroundXAxis(CameraCenterPoint.localRotation);
            //}
        }
    }
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -90f, 0f);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }
}
