using UnityEngine;
using System.Collections;
public class BallController : MonoBehaviour
{
    [SerializeField] private AudioSource collisonSoundSource;
    [SerializeField] private float maxVelocity, minXAxisVelocity, minYAxisVelocity;
    private Rigidbody2D body;
    private WaitForSeconds delay = new(1f);
    private void Start()
    {
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
    private void StartMove()
    {
        body.AddForce(new Vector2(Random.Range(0, 2) == 0 ? 0.1f : -0.1f, Random.Range(0, 2) == 0 ? Random.Range(0.1f, 0.15f) :
            Random.Range(-0.15f, -0.1f)), ForceMode2D.Impulse);
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
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        body.velocity = velocity;
    }
}
