using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Transform playerTarget;
    public float bulletSpeed;
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            other.GetComponent<PlayerController>().Dead();
            Destroy(gameObject);
        }
    }
}
