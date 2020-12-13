using UnityEngine;

public class BallController : MonoBehaviour
{
    private readonly float bounceStrength = 20;
    private Rigidbody ballRb;
    private AudioSource bounceSound;

    // Start is called before the first frame update
    void Start()
    {
        ballRb = GetComponent<Rigidbody>();
        bounceSound = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Use ForceMode.VelocityChange so the ball can bounce indefinitely
        if (collision.gameObject.CompareTag("Player"))
            ballRb.AddForce(Vector3.up * bounceStrength, ForceMode.VelocityChange);

        bounceSound.Play();
    }
}
