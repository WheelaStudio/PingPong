using UnityEngine;
public class UncontrolledBallController : MonoBehaviour
{
    private AudioSource collisionAudioSource;
    private void Start()
    {
        collisionAudioSource = GameObject.FindObjectOfType<AudioSource>();
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(0, 2) == 0 ? -0.1f : 0.1f,
            Random.Range(0, 2) == 0 ? Random.Range(-0.1f, -0.05f) : Random.Range(0.05f, 0.1f)), ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionAudioSource.Play();
    }
}
