using System.Collections;
using TMPro;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    private readonly float speed = 50;
    private Rigidbody playerRb;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        // Move in the direction of the user's touch
        if (gameManager.IsGameActive && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            int input = (touch.position.x >= Screen.width / 2) ? 1 : -1;
            playerRb.AddForce(Vector3.right * speed * input, ForceMode.Impulse);
        }
    }
}
