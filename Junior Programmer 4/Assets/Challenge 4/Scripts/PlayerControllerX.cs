using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private float dashSpeed = 10;  // ความเร็วในการ dash
    private float dashDuration = 0.1f; // ระยะเวลา dash สั้น
    private float dashCooldown = 4.0f; // ระยะเวลาที่ต้องรอก่อน dash ใหม่
    private bool canDash = true;   // สถานะการ dash ได้หรือไม่ได้
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            // เมื่อกด shift และ dash ได้ ให้เริ่ม dash
            StartCoroutine(Dash());
        }
        else
        {
            // เคลื่อนที่ปกติเมื่อไม่ dash
            playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime);
        }

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
    }

    // Coroutine สำหรับการ dash และ cooldown
    IEnumerator Dash()
    {
        canDash = false;  // ไม่สามารถ dash ได้อีกจนกว่าจะ cooldown
        Vector3 dashDirection = focalPoint.transform.forward;  // พุ่งไปข้างหน้าตามกล้อง

        // เพิ่มแรงกระทำอย่างรวดเร็ว
        playerRb.AddForce(dashDirection * dashSpeed, ForceMode.Impulse);

        // รอ dashDuration เพื่อหยุดการ dash
        yield return new WaitForSeconds(dashDuration);

        // รอ cooldown 4 วินาที
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;  // ทำให้ dash ได้อีกครั้ง
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }
}
