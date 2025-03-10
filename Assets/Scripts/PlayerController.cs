using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public float movementSpeed;

    public Animator animator;
    public readonly string moveAnimParameter = "Move";

    public DollController dollController;
    public bool isDead;

    public GameObject playerBody, playerRagdoll;
    public Transform ragdollHips;
    public CameraFollow cameraFollow;

    public ParticleSystem bloodEffect;

    private AudioSource audioSource;
    public AudioClip gotShotSfx;

    public bool hasWon;
    public GameObject youWinText;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isDead) return;
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, 0, moveZ);

        if (!hasWon)
        {
            if (moveX != 0 || moveZ != 0)
            {
                if (!dollController.isGreenLight)
                {
                    dollController.ShootPlayer(transform);
                    print("kamu ditembak mati");
                }
            }
        }

        characterController.Move(move * movementSpeed * Time.deltaTime);

        float moveAnim = new Vector2(moveX, moveZ).magnitude;
        animator.SetFloat(moveAnimParameter, moveAnim);

        if (moveX == 0 && moveZ == 0) return;
        float heading = Mathf.Atan2(moveX, moveZ);
        transform.rotation = Quaternion.Euler(0, heading * Mathf.Rad2Deg, 0);
    }

    public void Dead()
    {
        isDead = true;
        audioSource.PlayOneShot(gotShotSfx);
        bloodEffect.Play();
        cameraFollow.playerTarget = ragdollHips;
        playerBody.SetActive(false);
        playerRagdoll.SetActive(true);
        print("player mati");
        StartCoroutine(RestartGameCoroutine());
    }

    IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FinishLine>())
        {
            youWinText.SetActive(true);
            hasWon = true;
        }
    }
}
