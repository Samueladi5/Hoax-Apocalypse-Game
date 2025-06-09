using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    public Transform player;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRadius = 2f;
    public float patrolRadius = 5f;

    public GameObject signMarah;

    public GameObject animationPrefab;

    public GameObject losepanel;

    public GameObject circleDetect;
    public Animator enemyAnimator;

    [Header("Detection")]
    public SpriteRenderer detectionSprite;  

    private bool isChasing = false;
    private Vector3 patrolTarget;
    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;

    TypingManager typingManager;
    PauseManager pauseManager;


    void Start()
    {
        SetNewPatrolTarget();
        lastPosition = transform.position;
        animationPrefab.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();

        typingManager = FindObjectOfType<TypingManager>();
        pauseManager = FindObjectOfType<PauseManager>();

        losepanel.SetActive(false);
        Time.timeScale = 1;

        signMarah.SetActive(false);

        circleDetect.SetActive(true);

        // Set the size of the detection sprite based on the detection radius
        UpdateDetectionSprite();
    }

    void Update()
    {
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius && !typingManager.isInteracting)  // Only chase if not interacting
        {
            if (!isChasing)
            {
                isChasing = true;
                enemyAnimator.SetBool("isChasing", true);
                signMarah.SetActive(true);
                SfxAudioClip.Instance.PlayAudioAtIndex(SfxAudioClip.AudioCategory.Enemy, 0);
            }
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                enemyAnimator.SetBool("isChasing", false);
                signMarah.SetActive(false);
                SfxAudioClip.Instance.StopRandomAudio(SfxAudioClip.AudioCategory.Enemy);
            }
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        RotateBasedOnDirection();
    }

    void SetNewPatrolTarget()
    {
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);
        patrolTarget = new Vector3(
            transform.position.x + randomX,
            transform.position.y,
            transform.position.z + randomZ
        );
    }

    void Patrol()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            patrolTarget,
            patrolSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
        {
            SetNewPatrolTarget();
        }
    }

    void ChasePlayer()
    {
        // Chase player only if not interacting
        if (!typingManager.isInteracting)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                chaseSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, player.position) < 1.9f)
            {
                player.gameObject.SetActive(false);   // Disable player
                animationPrefab.SetActive(true);      // Activate animation prefab

                circleDetect.SetActive(false);
                signMarah.SetActive(false);

                SfxAudioClip.Instance.StopRandomAudio(SfxAudioClip.AudioCategory.Walking);

                detectionSprite.enabled = false;
                StartCoroutine(LosePanel());

                
            }
        }
        else
        {
            isChasing = false;
            enemyAnimator.SetBool("isChasing", false);

            Patrol();
        }
    }

    // Rotate the enemy based on its movement direction (left or right)
    void RotateBasedOnDirection()
    {
        Vector3 movementDirection = transform.position - lastPosition;

        if (movementDirection.z > 0)
        {
            spriteRenderer.flipX = true; // Face right
        }
        else if (movementDirection.z < 0)
        {
            spriteRenderer.flipX = false; // Face left
        }

        lastPosition = transform.position; // Update lastPosition after movement
    }

    // Update the size of the detection sprite to match the detection radius
    void UpdateDetectionSprite()
    {
        if (detectionSprite != null)
        {
            float diameter = detectionRadius * 2f; // Detection radius is the half of the full diameter
            detectionSprite.transform.localScale = new Vector3(diameter, diameter, 1f); // Adjust scale to match the radius
        }
    }

    // Ensure sprite size is updated in the editor
    void OnValidate()
    {
        UpdateDetectionSprite();
    }

    public IEnumerator LosePanel()
    {       
        yield return new WaitForSeconds(2f);

        losepanel.SetActive(true);
        pauseManager.isLosing = true;
        Time.timeScale = 0; 
    }
}
