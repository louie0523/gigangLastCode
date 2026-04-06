using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    public static Player Instance;

    public float Acc;

    public float RoateSpeed;

    public float Tilt;
    public float TiltSpeed = 5f;
    public float TiltReturnSpeed = 8f;
    public float currentTilt;
    public Vector3 CurrentVelocity;

    public Transform child;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();

        child = transform.GetChild(0).transform;

        Material = child.GetComponent<Renderer>().material;
        OriginColor = Material.color;
    }

    private void FixedUpdate()
    {
        if (!isAlive || isKnockBack)
            return;

        HandleMovement();
                
    }

    private void Update()
    {
        HandleRoate();
        HandleTilt();
    }

    void HandleMovement()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        float targetSpeed = v > 0 ? Speed :
            v < 0 ? -Speed * 0.3f :
            Speed * 0.5f;

        Vector3 targetmove = transform.forward * targetSpeed;

        CurrentVelocity = Vector3.Lerp(CurrentVelocity, targetmove, Time.fixedDeltaTime * Acc * (h == 0 && v == 0 ? 0.3f : 1f));

        rb.velocity = new Vector3(CurrentVelocity.x, 0f, CurrentVelocity.z);

        CameraTo();


    }

    void CameraTo()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);

        vp.x = Mathf.Clamp01(vp.x);
        vp.y = Mathf.Clamp01(vp.y);

        Vector3 World = Camera.main.ViewportToWorldPoint(vp);

        if (Vector3.Distance(transform.position, World) > 0.01f)
        {
            CurrentVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
        transform.position = World;
    }

    void HandleRoate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0f, RoateSpeed * Time.deltaTime * h, 0f);
    }

    void HandleTilt()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float TargetTilt = h != 0 ? Tilt * -h : 0f;
        float tiltSpeed = h != 0 ? TiltSpeed : TiltReturnSpeed;

        currentTilt = Mathf.Lerp(currentTilt, TargetTilt, tiltSpeed * Time.unscaledDeltaTime);

        child.localEulerAngles = new Vector3(child.localEulerAngles.x, child.localEulerAngles.y, currentTilt);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = null;
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy = collision.gameObject.GetComponent<Enemy>();
        }

        if (enemy == null || !enemy.isAlive)
            return;

        StartCoroutine(enemy.KnockBack(15f, 0.25f));

        
    }
}
