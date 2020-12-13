using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	private readonly float spawnRange = 18;
	private readonly float yBound = -10;
	private readonly float startDelay = 2;
    
	private float spawnDelay = 2;
	private GameManager gameManager;
	private PlayerController player;
	private GameObject ball;

	[SerializeField] private GameObject ballPrefab;
	[SerializeField] private GameObject skullPrefab;
	[SerializeField] private GameObject lifePrefab;
	[SerializeField] private List<GameObject> spawnItems;

	// Start is called before the first frame update
	void Start()
    {
		UnityEngine.Random.InitState(DateTime.Now.Millisecond);
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
	}

	// Update is called once per frame
	void Update()
    {
		if (gameManager.IsGameActive)
		{
			if (ball == null) // First spawn after hitting play
			{
				ball = Instantiate(ballPrefab, GetRandomSpawn(), Quaternion.identity);
				StartSpawning();
			}
			else if (ball.transform.position.y < yBound) // Player dropped the ball
			{
				Destroy(ball);
				player.LoseLife();

				// Spawn a new ball to continue play
				if (player.IsAlive)
					StartCoroutine(SpawnBall(1));
			}
		}

		this.DestroyOutOfBounds();
	}

	/// <summary>
	/// Starts spawning skulls and lives.
	/// </summary>
	public void StartSpawning()
	{
		CancelInvoke();
		InvokeRepeating("SpawnRandom", startDelay, spawnDelay);
		InvokeRepeating("SpeedUp", 15, 15); // Speed up every 15 seconds
	}

	/// <summary>
	/// Stops all spawning.
	/// </summary>
	public void StopSpawning()
	{
		CancelInvoke();
	}

	/// <summary>
	/// Spawns a random obstacle or power up
	/// </summary>
	private void SpawnRandom()
	{
		int itemId = UnityEngine.Random.Range(0, spawnItems.Count);
		Rigidbody randItem = Instantiate(spawnItems[itemId], GetRandomSpawn(), Quaternion.identity).GetComponent<Rigidbody>();
		
		// Make the objects spin as they fall
		randItem.AddTorque(Vector3.up * 75, ForceMode.VelocityChange);
		randItem.AddTorque(Vector3.right * 25, ForceMode.VelocityChange);
	}

	/// <summary>
	/// Spawns a ball after a given delay as long as the game is still active
	/// and no other balls exist
	/// </summary>
	private IEnumerator SpawnBall(float delay)
	{
		yield return new WaitForSeconds(delay);

		if (ball == null && gameManager.IsGameActive)
			ball = Instantiate(ballPrefab, GetRandomSpawn(), Quaternion.identity);
	}

	private void SpeedUp()
	{
		Debug.Log("Speeding Up!");
		CancelInvoke();

		spawnDelay /= 1.5f;
		InvokeRepeating("SpawnRandom", startDelay, spawnDelay);

		// Increase pitch to give the effect of speeding up the music
		GameObject.Find("AudioSource").GetComponent<AudioSource>().pitch *= 1.5f;
	}

	private void DestroyOutOfBounds()
	{
		GameObject life = GameObject.FindWithTag("Life");
		if (life && life.transform.position.y < yBound)
			Destroy(life);

		GameObject badObj = GameObject.FindWithTag("Bad");
		if (badObj && badObj.transform.position.y < yBound)
			Destroy(badObj);
	}

	private Vector3 GetRandomSpawn()
	{
		float posX = 0;
		int loopTimes = 3;
		float range = this.spawnRange / loopTimes;

		// Bias towards 0 so that items fall near the centre more often
		for (int i = 0; i < loopTimes; i++)
			posX += UnityEngine.Random.Range(-range, range);

		return new Vector3(posX, 20, -1);
	}
}
