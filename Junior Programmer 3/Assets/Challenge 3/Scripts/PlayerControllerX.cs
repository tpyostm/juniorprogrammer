using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound; // New bounce sound

    public bool lowEnough = true;

    public float groundThreshold = 1.0f; // Threshold for detecting ground collision
    public float bounceForce = 10.0f;    // Force applied when the player bounces off the ground

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // Clamp the player's y position to 20 so they can't go beyond that
        transform.position = new Vector3(transform.position.x, Mathf.Min(transform.position.y, 15), transform.position.z);

        // Check if the player's y position is greater than 20, and set lowEnough accordingly
        if (transform.position.y >= 15)
        {
            lowEnough = false;
        }
        else
        {
            lowEnough = true;
        }

        // While space is pressed, the game is not over, and the player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && lowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        // If the player is near the ground (y position < groundThreshold), bounce
        if (transform.position.y < groundThreshold)
        {
            BounceOffGround();
        }
    }

    private void BounceOffGround()
    {
        // Reverse the player's vertical velocity to simulate a bounce
        playerRb.velocity = new Vector3(playerRb.velocity.x, bounceForce, playerRb.velocity.z);

        // Play bounce sound effect
        playerAudio.PlayOneShot(bounceSound, 1.0f);
    }

    private void OnCollisionEnter(Collision other)
    {
        // If player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }
        // If player collides with money, play fireworks and destroy money
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
    }
}
