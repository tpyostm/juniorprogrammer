using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinPropellerX : MonoBehaviour
{
    // ��������㹡����ع
    public float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // ��ع�ѵ���ͺ᡹ Z (���͵��᡹���س��ͧ���)
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}