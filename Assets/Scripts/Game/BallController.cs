using UnityEngine;
using System.Collections;
public class BallController : MonoBehaviour
{
    [SerializeField] private AudioSource collisonSoundSource;
    [SerializeField] private float minXAxisVelocity, minYAxisVelocity, minDefaultSpeed, differenceSpeed, speedIncreaceCoefficient;
    private TrailRenderer trailRenderer;
    private float minSpeed, maxSpeed;
    public float Speed { get; private set; }
    private Rigidbody2D body;
    private readonly WaitForSeconds resetPositionDelay = new(1f);
    private readonly WaitForSeconds increaseSpeedDelay = new(120f);
    public static BallController Shared { get; private set; }
    public bool IsOnTheField { get; private set; } = true;
    public delegate void ExitFromTheField();
    public event ExitFromTheField OnExitFromTheField;
    [HideInInspector]
    private void Awake()
    {
        Shared = this;
    }
    private void Start()
    {
        minSpeed = minDefaultSpeed * (Preferences.DistanceBetweenFlats / Preferences.ScreenWorldHeight);
        Speed = minSpeed;
        maxSpeed = minSpeed + differenceSpeed;
        body = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
        if (Preferences.CurrentGameDesign == GameDesign.Atari)
            Destroy(trailRenderer);
        StartMove();
        StartCoroutine(IncreaseSpeedTimer());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisonSoundSource.Play();
    }
    private IEnumerator IncreaseSpeedTimer()
    {
        while(true)
        {
            yield return increaseSpeedDelay;
            Speed = Mathf.Clamp(Speed + speedIncreaceCoefficient, minSpeed, maxSpeed);
            if (Speed == maxSpeed)
            {
                break;
            }
        }
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        IsOnTheField = false;
        OnExitFromTheField.Invoke();
        yield return resetPositionDelay;
        transform.position = Vector2.zero;
        body.velocity = Vector2.zero;
        if (Preferences.CurrentGameDesign == GameDesign.Common)
            trailRenderer.Clear();
        IsOnTheField = true;
        StartMove();
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
        body.velocity = velocity.normalized * Speed;
    }
}
