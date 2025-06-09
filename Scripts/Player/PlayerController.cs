using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool canMove = true;
    private float originalMoveSpeed;  // Simpan kecepatan asli
    private Vector3 movement;
    private bool isSprinting = false;
    private bool sprintOnCooldown = false;
    private bool isAttacking = false;

    [Header("Energy")]
    public float maxEnergy = 100f;  
    public float currentEnergy;     
    public float sprintEnergyCost = 10f;  
    public float energyRecoveryRate = 5f; 

    [Header("UI")]
    public Image energyCircle;

    bool isPlayingWalkingAudio = false;



    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        originalMoveSpeed = moveSpeed;  // Simpan kecepatan asli saat memulai
        currentEnergy = maxEnergy;  
        UpdateEnergyUI();
    }

    void Update()
    {
        if (canMove) // Check if movement is allowed
        {
            movement = Vector3.zero;

            // Basic movement inputs
            if (Input.GetKey(KeyCode.A)) { movement.z = 1; }
            if (Input.GetKey(KeyCode.D)) { movement.z = -1; }
            if (Input.GetKey(KeyCode.S)) { movement.x = -1; }
            if (Input.GetKey(KeyCode.W)) { movement.x = 1; }

            // Sprint logic
            bool isShiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (isShiftPressed && currentEnergy > 0 && !sprintOnCooldown)
            {
                StartSprint();
            }
            else
            {
                StopSprint();
            }

            movement *= moveSpeed;
            UpdateAnimation();
            UpdateEnergyUI();
        }
    }

    private void StartSprint()
    {
        isSprinting = true;
        animator.SetBool("isRunning", true);

        energyCircle.gameObject.SetActive(true);

        currentEnergy -= sprintEnergyCost * Time.deltaTime;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (currentEnergy <= 0)
        {
            StopSprint();  
        }
        else
        {
            // Gandakan kecepatan hanya sekali dengan mengatur moveSpeed menjadi 2x dari kecepatan asli
            moveSpeed = originalMoveSpeed * 2;
        }
    }

    private void StopSprint()
    {
        if (isSprinting)
        {
            isSprinting = false;
            moveSpeed = originalMoveSpeed;  // Kembalikan kecepatan ke nilai asli
            animator.SetBool("isRunning", false);
        }

        if (currentEnergy < maxEnergy)
        {
            currentEnergy += energyRecoveryRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);  
        }
    }

    private void FixedUpdate()
    {
        if (!isAttacking && canMove) 
        {
            Move();
        }

        if (movement == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void Move()
    {
        Vector3 moveDirection = movement.normalized;
        Vector3 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);

        if (moveDirection.z < 0) { transform.localScale = new Vector3(-.57f, .57f, .57f); }
        else if (moveDirection.z > 0) { transform.localScale = new Vector3(.57f, .57f, .57f); }
    }

    public void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true); 
    }

    public void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false); 
    }

    void UpdateAnimation()
    {
        if (!isAttacking)
        {
            if (movement != Vector3.zero)
            {
                animator.SetBool("isWalking", true);

                // Hanya putar audio sekali saat mulai berjalan
                if (!isPlayingWalkingAudio)
                {
                    SfxAudioClip.Instance.PlayLoopingAudio(SfxAudioClip.AudioCategory.Walking);
                    isPlayingWalkingAudio = true;
                }
            }
            else
            {
                animator.SetBool("isWalking", false);

                // Hentikan audio saat berhenti berjalan
                if (isPlayingWalkingAudio)
                {
                    SfxAudioClip.Instance.StopRandomAudio(SfxAudioClip.AudioCategory.Walking);
                    isPlayingWalkingAudio = false;
                }
            }
        }
    }

    void UpdateEnergyUI()
    {
        float fillValue = currentEnergy / maxEnergy;
        energyCircle.fillAmount = fillValue;
    }
}
