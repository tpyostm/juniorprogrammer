using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropellerX : MonoBehaviour
{
    // ความเร็วในการหมุน
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // หมุนวัตถุรอบแกน Z (หรือตามแกนที่คุณต้องการ)
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}