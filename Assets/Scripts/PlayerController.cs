using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int lives = 3;
    private bool canLoseLives = true;
    private Rigidbody playerRb;
    private SpawnManager spawner;
    private GameManager gameManager;
    private AudioSource audioSource;

    [SerializeField] private AudioClip lifeSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private ParticleSystem lifeParticle;
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private TextMeshProUGUI livesText;

    public bool IsAlive => this.lives > 0;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        spawner = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        audioSource = GetComponent<AudioSource>();
        livesText.text = $"Lives: {lives}";
    }

    /// <summary>
    /// Deducts a life from the player if possible
    /// </summary>
    public void LoseLife()
    {
        if (canLoseLives)
        {
            livesText.text = $"Lives: {--lives}";
            if (!IsAlive)
                gameManager.GameOver();
        }
    }

    private IEnumerator DestroyPlayer()
    {
        // Player should not lose lives while in death animation
        canLoseLives = false;
        spawner.StopSpawning();

        // Start death animation
        deathParticle.Play();
        audioSource.PlayOneShot(deathSound);
        playerRb.useGravity = true;
        playerRb.constraints = RigidbodyConstraints.None;

        // Wait for animation to complete
        yield return new WaitForSeconds(1.5f);

        // Reset player and start spawning again
        Reset();

        if (gameManager.IsGameActive)
            spawner.StartSpawning();

        // Give a little breathing room after respawning
        yield return new WaitForSeconds(1.5f);
        canLoseLives = true;
    }

    private void Reset()
    {
        // Ensure no bad object remains before resetting player position
        GameObject obstacle = GameObject.FindWithTag("Bad");
        if (obstacle)
            Destroy(obstacle);

        // TODO: make this less ugly
        deathParticle.Stop();
        playerRb.useGravity = false;
        playerRb.constraints = RigidbodyConstraints.FreezePositionY 
            | RigidbodyConstraints.FreezePositionZ 
            | RigidbodyConstraints.FreezeRotation;
        playerRb.velocity = new Vector3(0, 0, 0);
        playerRb.angularVelocity = new Vector3(0, 0, 0);
        playerRb.transform.rotation = new Quaternion(0, 0, 0, 0);
        playerRb.transform.position = new Vector3(0, -8, -1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collisions with obstacles
        if (collision.gameObject.CompareTag("Bad"))
        {
            if (canLoseLives)
            {
                LoseLife();
                StartCoroutine(DestroyPlayer());
            }
            else
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle collisions with bonus lives
        if (other.gameObject.CompareTag("Life"))
        {
            Destroy(other.gameObject);

            audioSource.PlayOneShot(lifeSound);
            lifeParticle.Play();
            livesText.text = $"Lives: {++lives}";
        }
    }
}
