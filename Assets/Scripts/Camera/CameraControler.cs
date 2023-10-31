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
    Vector3 zero;
    bool moving;
    Vector3 target;
    Solider CurrentlyFollowing;
    public static CameraControler LocalInstance { get; private set; }
    void Awake()
    {
        cam = Camera.main;
        LocalInstance = this;
        defaultRotation = transform.rotation;
        zero = new Vector3 (0, 0, 0);
        target = new Vector3(0, 0, 0);
    }


    // Update is called once per frame
    void Update()
    {
        float scroll = Input.mouseScrollDelta.y * 80;
        float newY = transform.position.y - scroll * 10 * Time.deltaTime;
        newY = Mathf.Clamp(newY, 50, 200); // Ensure the new Y position is within the specified limits
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);




        int speedtrue = speed;

        
        
        HandleRotation();
    
        Vector3 motion = GetMotionInput();

        MoveCamera(motion);

        if (moving)
        {
            if (!CurrentlyFollowing.seen)
                moving = false;
            else
            {
                SetCameraUnit(CurrentlyFollowing.transform.position);
                return;
            }
        }
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
        
        if (isRotating)
        {
            horizontalInput = 0;
            verticalInput= 0;
        }

        // Calculate the motion vector using camera orientation
        Vector3 motion = (cameraForward * verticalInput) + (cameraRight * horizontalInput);
        motion.Normalize();
        cam2.transform.position = transform.position;
        cam2.transform.rotation = transform.rotation;

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

    //change to take rotation into account in future
    public void SetCameraUnit (Vector3 position)
    {
        Vector3 Endpoint = getEndPosition(position);
        if (Endpoint != zero)
        {
            transform.position = Endpoint;
            
        }
        else
            GoDefaultstaet(position, speed);
    }

    public void SetCameraUnit(Vector3 position, int speedT)
    {

        Vector3 Endpoint = getEndPosition(position);
        if (Endpoint != zero)
        {
            StopCoroutine("CameraMoveSmooth");
            new WaitForSeconds(0.1f);
            StartCoroutine(CameraMoveSmooth(Endpoint, speedT));
        }
        else
            GoDefaultstaet(position, speedT);

       
    }

    //get position camera should go relative to current rotation and positon
    Vector3 getEndPositionC(Vector3 position) {

        Vector3 Endpoint = new Vector3(0, 0, 0);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            Endpoint = hit.point * 1.02f;
            Debug.Log(Vector3.Distance(hit.point, transform.position));
            Endpoint -= transform.position;
            Endpoint = position - Endpoint;
            //Endpoint.y = position.y + 80;

        }


        return Endpoint;
    }

    Vector3 getEndPosition(Vector3 position)
    {

        Vector3 Endpoint = new Vector3(0, 0, 0);
        float distance = 153; // Desired distance

        Vector3 direction = transform.forward;
        Endpoint = transform.position + (direction * distance);

        Endpoint -= transform.position;
        Endpoint = position - Endpoint;


        return Endpoint;
    }

    public void FollowUnit(Solider unit)
    {
        moving = true;
        CurrentlyFollowing = unit;
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



    void GoDefaultstaet(Vector3 position, int speedT)
    {
        position.x -= 125;
        position.y += 80;
        position.z -= 105;
        //55
        //10
        //55

        //-29.27 90 - 25.5
        StopCoroutine(CameraMoveSmooth(position, speedT));
        StartCoroutine(CameraMoveSmooth(position, speedT));
        transform.rotation = defaultRotation;
    }



    public void goTOcurrent()
    {
        SetCameraUnit(ObjectSelector.Instance.getSelectedUnit().transform.position);
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
