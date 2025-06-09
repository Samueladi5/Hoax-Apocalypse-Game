using System.Collections;
using UnityEngine;

public class PeopleMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float movementRange = 5f;
    public float idleTime = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    public bool isMoving = true;

    private Coroutine movementCoroutine;
    public Animator animator;
    public Rigidbody RB;
    private SpriteRenderer spriteRenderer;

    public GameObject InteractIndicator;

    // Private SphereCollider
    private SphereCollider sphereCollider;

    void Start()
    {
        initialPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        movementCoroutine = StartCoroutine(MoveRandomly());

        InteractIndicator.SetActive(false);

        // Assign the existing SphereCollider in the object
        sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            Debug.LogError("No SphereCollider attached to the object.");
        }
    }

    public IEnumerator MoveRandomly()
    {
        while (true)
        {
            if (!isMoving)
            {
                float randomX = Random.Range(-movementRange, movementRange);
                float randomZ = Random.Range(-movementRange, movementRange);
                targetPosition = new Vector3(initialPosition.x + randomX, initialPosition.y, initialPosition.z + randomZ);

                isMoving = true;

                if (randomZ >= 0)
                {
                    spriteRenderer.flipX = false;
                    FlipCollider(true);
                }
                else if (randomZ <= 0)
                {
                    spriteRenderer.flipX = true;
                    FlipCollider(false);
                }

                while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                    yield return null;
                }

                isMoving = false;
                yield return new WaitForSeconds(idleTime);
            }
        }
    }

    private void FlipCollider(bool isFlipped)
    {
        if (sphereCollider != null)
        {
            if (isFlipped)
            {
                sphereCollider.center = new Vector3(4.16f, 1.32f, 0f);
            }
            else
            {
                sphereCollider.center = new Vector3(-4.16f, 1.32f, 0f);
            }
        }
    }

    public void StopMovement()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            isMoving = false;
        }
    }

    public void ResumeMovement()
    {
        if (movementCoroutine == null)
        {
            movementCoroutine = StartCoroutine(MoveRandomly());
        }
    }

    public void OnDisable()
    {
        StopMovement();
    }
}
