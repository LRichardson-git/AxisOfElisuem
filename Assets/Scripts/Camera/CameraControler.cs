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
    public Camera cam2;
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
        float scroll = Input.mouseScrollDelta.y * 80;
        float newY = transform.position.y - scroll * speed * Time.deltaTime;
        newY = Mathf.Clamp(newY, 50, 200); // Ensure the new Y position is within the specified limits
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);








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
        cam2.transform.position = transform.position;
        cam2.transform.rotation = transform.rotation;
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

    //change to take rotation into account in future
    public void SetCameraUnit (Vector3 position)
    {
        
        position.x -= 125;
        position.y += 80;
        position.z -= 105;
        //55
        //10
        //55

        //-29.27 90 - 25.5
        StartCoroutine(CameraMoveSmooth(position,0));
        transform.rotation = defaultRotation;
    }

    public void SetCameraUnit(Vector3 position, int speedT)
    {

        position.x -= 125;
        position.y += 80;
        position.z -= 105;
        //55
        //10
        //55

        //-29.27 90 - 25.5
        StartCoroutine(CameraMoveSmooth(position, speedT));
        transform.rotation = defaultRotation;
    }




    IEnumerator CameraMoveSmooth(Vector3 target, int possilbeSpeed)
    {

        int speedtrue = speed;

        if (possilbeSpeed != 0)
            speedtrue = possilbeSpeed;

        while (Vector3.Distance(transform.position, target) > 10)
        {
            
            transform.position = Vector3.MoveTowards(transform.position,target, (speedtrue * 2) * Time.deltaTime);
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
