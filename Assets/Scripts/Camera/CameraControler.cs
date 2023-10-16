using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam;
    public int speed = 20;
    Vector3 cameraForward;
    Vector3 cameraRight;
    public float rotationSpeed = 1f;
    private bool isRotating;
    private float rotationX;
    private Vector3 rotationPoint;
    private Quaternion defaultRotation;
    public float Speed = 10;
    public static CameraControler LocalInstance { get; private set; }
    void Awake()
    {
        cam = Camera.main;
        LocalInstance = this;
        defaultRotation = transform.rotation;
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 motion = GetMotionInput();
        HandleRotation();
        MoveCamera(motion);
    }

    private Vector3 GetMotionInput()
    {
        cameraForward = cam.transform.forward;
        cameraForward.y = 0; // Ignore the Y-axis to move in the XZ plane
        cameraForward.Normalize();

        cameraRight = cam.transform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Calculate the horizontal and vertical inputs relative to camera orientation
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Calculate the motion vector using camera orientation
        Vector3 motion = (cameraForward * verticalInput) + (cameraRight * horizontalInput);
        motion.Normalize();

        return motion;
    }

    public void MoveCamera(Vector3 motion)
    {
        transform.Translate(motion * speed * Time.deltaTime, Space.World);
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
    }

    public void MoveCamera(Vector3 motion, float speed)
    {
        transform.Translate(motion * speed * Time.deltaTime, Space.World);
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
    }

    public void MoveCamera(Vector3 motion, float speed, Quaternion rotation)
    {
        transform.Translate(motion * speed * Time.deltaTime, Space.World);
        cam.transform.position = transform.position;
        cam.transform.rotation = rotation;
    }

    public void SetCamera (Vector3 position)
    {

        StartCoroutine(CameraMoveSmooth(position));
    }

    IEnumerator CameraMoveSmooth(Vector3 target)
    {
       
        while (Vector3.Distance(transform.position, target) > 10)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, target, Time.deltaTime * Speed);
            transform.position = smoothedPosition;
            yield return null;
        }
    }









    private void HandleRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            isRotating = true;
            rotationPoint = FindRotationPoint();
            rotationX = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonUp(2))
        {
            isRotating = false;
        }


        if (isRotating)
        {
            float mouseX = Input.mousePosition.x;
            float rotationDelta = mouseX - rotationX;

            // Adjust the rotation speed according to the delta
            float rotationAmount = rotationDelta * rotationSpeed;

            // Rotate the camera around the rotation point
            Vector3 targetPos = transform.position - rotationPoint;
            Quaternion rotation = Quaternion.Euler(0f, rotationAmount, 0f);
            targetPos = rotation * targetPos;
            transform.position = rotationPoint + targetPos;

            // Keep the camera looking at the rotation point
            transform.LookAt(rotationPoint);

            rotationX = mouseX;
        }
    }

    private Vector3 FindRotationPoint()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return transform.position;
    }
}
