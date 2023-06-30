using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{
    private enum LosingPlayer
    {
        Left, Right, Unknown
    }
    private LosingPlayer _currentLosingPlayer = LosingPlayer.Unknown;
    private const float MINIMUM_BALL_SPEED_FACTOR = 0.95f;
    private const float POSITION_DIFFERENCE_COEFFICIENT = 0.2f;
    [SerializeField] private AudioSource commonCollisonSound, atariBeep, atariPeep, atariPlop;
    [SerializeField] private float minXAxisVelocity, minYAxisVelocity, minDefaultSpeed, differenceSpeed, speedIncreaceCoefficient;
    private float movementSpeed;
    private TrailRenderer trailRenderer;
    private float minSpeed, maxSpeed;
    public float Speed { get; private set; }
    public float SpeedCoefficient { get; private set; } = 1f;
    private Rigidbody2D body;
    private readonly WaitForSeconds resetPositionDelay = new(1f);
    private readonly WaitForSeconds increaseSpeedDelay = new(90f);
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
        movementSpeed = Speed;
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
        var collGameobject = collision.gameObject;
        var is_Flat = collGameobject.CompareTag("Flat");
        if (is_Flat)
        {
            SpeedCoefficient = MINIMUM_BALL_SPEED_FACTOR + Mathf.Abs(Mathf.Abs(transform.position.y) -
                Mathf.Abs(collGameobject.transform.position.y)) * POSITION_DIFFERENCE_COEFFICIENT;
            movementSpeed =
               SpeedCoefficient * Speed;
        }
        if (Preferences.CurrentGameDesign == GameDesign.Common)
            commonCollisonSound.Play();
        else
        {
            if (is_Flat)
                atariBeep.Play();
            else if (collGameobject.CompareTag("Border"))
                atariPlop.Play();
        }
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
        if (Preferences.CurrentGameDesign == GameDesign.Atari)
            atariPeep.Play();
        _currentLosingPlayer = collision.gameObject.name == "leftEdge" ? LosingPlayer.Left : LosingPlayer.Right;
        IsOnTheField = false;
        OnExitFromTheField.Invoke();
        yield return resetPositionDelay;
        movementSpeed = Speed;
        transform.position = Vector2.zero;
        body.velocity = Vector2.zero;
        if (Preferences.CurrentGameDesign == GameDesign.Common)
            trailRenderer.Clear();
        IsOnTheField = true;
        StartMove();
    }
    private void StartMove()
    {
        float x;
        if (_currentLosingPlayer == LosingPlayer.Unknown)
            x = Random.Range(0, 2) == 0 ? 0.1f : -0.1f;
        else
            x = _currentLosingPlayer == LosingPlayer.Left ? -0.1f : 0.1f;
        body.AddForce(new Vector2(x, Random.Range(0, 2) == 0 ? Random.Range(0.05f, 0.15f) :
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
        body.velocity = velocity.normalized * movementSpeed;
    }
}
