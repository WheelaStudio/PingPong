using UnityEngine;
public class BallController : MonoBehaviour
{
    [SerializeField] private AudioSource collisonSoundSource;
    private Rigidbody2D body;
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.AddForce(new Vector2(Random.Range(0, 2) == 0 ? -0.1f : 0.1f, Random.Range(0, 2) == 0 ? -0.1f : 0.1f), ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisonSoundSource.Play();
    }
}
