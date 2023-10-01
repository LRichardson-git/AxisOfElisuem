using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public Vector3 targetLocation;
    public float speed = 10.0f;
    public AudioClip bounceSound;
    public float bounceForce = 10.0f;
    private AudioSource audioSource;
    private Rigidbody rb;
    InputManager inputManager;
    bool Aiming = false;
    bool fire = false;


    LineRenderer _Line;


    [SerializeField] float _initVelocity;
    [SerializeField] float _angle;
    [SerializeField ] float step;
    //float height;



    Vector3 firepoint;



    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        _Line = GetComponent<LineRenderer>();
        inputManager = InputManager.Instance;
    }

    public void setTarget(Vector3 Start, Vector3 target) { transform.position = Start; targetLocation = target;  firepoint = Start; }


    public void Aim(Vector3 firepoint)
    {

        this.firepoint = firepoint;


        Vector3 target = Helper_Functions.getWorldMouse().point;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            target = hit.point;
        }
        


        Vector3 direction = target - firepoint;
        Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);


        float height = target.y + target.magnitude / 2f;
        height = Mathf.Max(0.01f, height);
        float angle;
        float v0;
        float time;
        CalculatePathHeight(targetPos, height, out v0, out angle, out time);
        DrawPath(groundDirection.normalized, v0, angle, time, step);
    }


    private float QuadraticEqua(float a, float b, float c, float sign)
    {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    [System.Obsolete]
    private void DrawPath(Vector3 direction, float v0, float angle, float time, float step) {

        step = Mathf.Max(0.01f, step);
        _Line.positionCount = (int)(time / step) + 2;
        int count = 0;
        
        for (float i = 0; i < time; i+= step){

            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _Line.SetPosition(count, firepoint + direction * x + Vector3.up * y);
            count++;

            if (i > 1)
            {
                RaycastHit hit;


                if (Physics.Raycast(_Line.GetPosition((int)i), _Line.GetPosition((int)i - 1) - _Line.GetPosition((int)i), out hit, step))
                {
                    if (hit.transform.CompareTag("wall"))
                    {
                        _Line.SetColors(Color.black, Color.black);
                    }
                }
            }

            }

        float xfinal = v0 * time * Mathf.Cos(angle);
        float yfinal = v0 * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _Line.SetPosition(count, firepoint + direction * xfinal + Vector3.up * yfinal);
        _Line.endColor = Color.yellow;
    }

    private void CalculatePathHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;
        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEqua(a, b, c, 1);
        float tmin = QuadraticEqua(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b * time / xt);

        v0 =b/Mathf.Sin(angle);
    }


    public bool CheckFire(Vector3 start, Vector3 target)
    {
        Vector3 middlepos = new Vector3 (0,0,0);
        for (int i = 0; i < _Line.positionCount / 2; i++)
        {
            middlepos = _Line.GetPosition(i);
        }
        Vector3 direction = middlepos - start;
        float distance = Vector3.Distance(start, middlepos);
        if (!Physics.Raycast(start, direction, out RaycastHit hitInfoC, distance))
        {
            direction = target - middlepos;
            distance = Vector3.Distance(target, middlepos) - 1;
            if (!Physics.Raycast(middlepos, direction, out RaycastHit hitInfoD, distance))
            {
                return true;
            }

        }





            


        return false;
    }



    public void fireF(Vector3 target, GrenadeAbility ability)
    {


        Vector3 direction = target - firepoint;
        Vector3 groundDirection = new Vector3(direction.x, 0, direction.z);
        Vector3 targetPos = new Vector3(groundDirection.magnitude, direction.y, 0);


        float height = target.y + target.magnitude / 2f;
        height = Mathf.Max(0.01f, height);
        float angle;
        float v0;
        float time;
        CalculatePathHeight(targetPos, height, out v0, out angle, out time);

        StopAllCoroutines();
        StartCoroutine(Coroutine_Movement(groundDirection.normalized,v0, angle, time,ability, target));
    }
    IEnumerator Coroutine_Movement(Vector3 direction,float v0, float angle, float time, GrenadeAbility ability, Vector3 target)
    {
        float t = 0;

        while (t < time)
        {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - (1f/2f) *  -Physics.gravity.y * Mathf.Pow(t,2);
            transform.position = firepoint + direction * x + Vector3.up * y;
            t += Time.deltaTime * speed;
            yield return null;
        }

        ability.ExcuteAbility(target);
    }


    



    private void OnCollisionEnter(Collision collision)
    {
        // Play the bouncing sound effect
        audioSource.PlayOneShot(bounceSound);

        // Apply a force to the grenade to make it bounce
       // rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
    }

    private void Explode()
    {
        // Play the explosion sound effect
       // audioSource.PlayOneShot(explosionSound);

        // Destroy the grenade
       // Destroy(gameObject);
    }


}
