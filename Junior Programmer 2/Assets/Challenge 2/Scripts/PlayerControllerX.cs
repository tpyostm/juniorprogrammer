using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    private float cooldownTime = 0.5f;  // เวลาคูลดาวน์ 1 วินาที
    private float nextFireTime = 0;  // เวลาที่สามารถกด Spacebar ได้อีกครั้ง

    // Update is called once per frame
    void Update()
    {
        // ตรวจสอบว่าปัจจุบันเวลามากกว่าเวลาที่สามารถกด Spacebar ได้อีกครั้งหรือไม่
        if (Time.time > nextFireTime)
        {
            // เมื่อกด Spacebar และคูลดาวน์เสร็จสิ้น
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // สร้างสุนัขที่ตำแหน่งของผู้เล่น
                Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);

                // กำหนดเวลาคูลดาวน์ใหม่
                nextFireTime = Time.time + cooldownTime;
            }
        }
    }
}
