using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public float minTimer, maxTimer;
    public bool isGreenLight = true;

    public Animator animator;
    public readonly string greenLightAnim = "GreenLight";

    public Transform shotPoint;
    public GameObject bulletPrefab;
    public bool hasShot;

    private AudioSource audioSource;
    public AudioClip redLightSfx, greenLightSfx, shootSfx;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(ChangeLightCoroutine());
    }

    IEnumerator ChangeLightCoroutine()
    {
        yield return  new WaitForSeconds(Random.Range(minTimer, maxTimer));

        if (isGreenLight)
        {
            animator.SetBool(greenLightAnim, false);
            audioSource.PlayOneShot(redLightSfx);
            yield return new WaitForSeconds(0.7f);
            isGreenLight = false;
            print("lampu merah, player gaboleh jalan!");
        }
        else
        {
            audioSource.PlayOneShot(greenLightSfx);
            isGreenLight = true;
            animator.SetBool(greenLightAnim, true);
            print("lampu hijau, player boleh jalan!");
        }
        StartCoroutine(ChangeLightCoroutine());
    }

    public void ShootPlayer(Transform playerTarget)
    {
        if (hasShot) return;
        audioSource.PlayOneShot(shootSfx);
        GameObject bulletGO = Instantiate(bulletPrefab, shotPoint.position, Quaternion.identity);
        bulletGO.GetComponent<BulletMovement>().playerTarget = playerTarget;
        hasShot = true;
    }
}
