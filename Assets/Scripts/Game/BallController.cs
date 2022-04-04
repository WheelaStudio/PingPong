using UnityEngine;
using System.Collections;
public class BallController : MonoBehaviour
{
    [SerializeField] private AudioSource collisonSoundSource;
    [SerializeField] private float minXAxisVelocity, minYAxisVelocity, minDefaultSpeed, differenceSpeed, speedIncreaceCoefficient;
    private float minSpeed, maxSpeed, speed;
    private Rigidbody2D body;
    private readonly WaitForSeconds delay = new(1f);
    public static BallController Shared { get; private set; }
    private const float screenHeight = 10f;
    public bool IsVisible { get; private set; }
    [HideInInspector] public float distanceBetweenFlats;
    private void Awake()
    {
        Shared = this;
    }
    private void Start()
    {
        minSpeed = minDefaultSpeed * (distanceBetweenFlats / screenHeight);
        maxSpeed = minSpeed + differenceSpeed;
        speed = minSpeed;
        body = GetComponent<Rigidbody2D>();
        StartMove();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisonSoundSource.Play();
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        yield return delay;
        body.position = Vector2.zero;
        body.velocity = Vector2.zero;
        StartMove();
    }
    public void IncreaseSpeed()
    {
        speed = Mathf.Clamp(speed + speedIncreaceCoefficient, minSpeed, maxSpeed);
    }
    private void StartMove()
    {
        body.AddForce(new Vector2(Random.Range(0, 2) == 0 ? 0.1f : -0.1f, Random.Range(0, 2) == 0 ? Random.Range(0.05f, 0.15f) :
            Random.Range(-0.15f, -0.05f)), ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        var velocity = body.velocity;
        if (velocity.x < minXAxisVelocity && velocity.x > 0f)
            velocity.x = minXAxisVelocity;
        if (velocity.x > -minXAxisVelocity && velocity.x <= 0f)
            velocity.x = -minXAxisVelocity;
        if (velocity.y < minYAxisVelocity && velocity.y > 0f)
            velocity.y = minYAxisVelocity;
        if (velocity.y > -minYAxisVelocity && velocity.y <= 0f)
            velocity.y = -minYAxisVelocity;
        body.velocity = velocity.normalized * speed;
    }
    private void OnBecameInvisible()
    {
        IsVisible = false;
    }
    private void OnBecameVisible()
    {
        IsVisible = true;
    }
}
