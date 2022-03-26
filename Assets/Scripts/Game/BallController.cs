using UnityEngine;
using System.Collections;
public class BallController : MonoBehaviour
{
    [SerializeField] private AudioSource collisonSoundSource;
    [SerializeField] private float maxVelocity, minXAxisVelocity;
    private Rigidbody2D body;
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
        yield return new WaitForSeconds(1f);
        body.position = Vector2.zero;
        body.velocity = Vector2.zero;
        StartMove();
    }
    private void StartMove()
    {
        body.AddForce(new Vector2(Random.Range(0, 2) == 0 ? -0.1f : 0.1f, Random.Range(0, 2) == 0 ? Random.Range(-0.25f, -0.1f) : Random.Range(0.1f, 0.25f)), ForceMode2D.Impulse);
    }
    private void FixedUpdate()
    {
        var velocity = body.velocity;
        velocity = Vector2.ClampMagnitude(velocity, maxVelocity);
        if (velocity.x < minXAxisVelocity && velocity.x > 0f)
            velocity.x = minXAxisVelocity;
        if (velocity.x > -minXAxisVelocity && velocity.x < 0f)
            velocity.x = -minXAxisVelocity;
        body.velocity = velocity;
    }
}
