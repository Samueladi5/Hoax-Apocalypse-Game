using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    List<GameObject> npcsInRange = new List<GameObject>();
    GameObject closestNPC;

    TypingManager TypingManager;
    PlayerController playerController;
    PeopleMovement ClosestNpcPF;
    SpawnManager spawnManager;
    

    private void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
        TypingManager = FindObjectOfType<TypingManager>();
        playerController = FindObjectOfType<PlayerController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && closestNPC != null)
        {
            StartInteraction();
            if(!TypingManager.isZooming)
            {
                StartCoroutine(TypingManager.ZoomInCamera());
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Victim"))
        {
            Debug.Log("Contac Wirh" + other.gameObject);
            GameObject npc = other.gameObject;

            if (npc != null)
            {
                npcsInRange.Add(npc);
                UpdateClosestNPC();
                PeopleMovement movementComponent = npc.GetComponent<PeopleMovement>();
                if (movementComponent != null)
                {
                    movementComponent.InteractIndicator.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Victim"))
        {
            GameObject npc = other.gameObject;

            if (npc != null && npcsInRange.Contains(npc))
            {
                npcsInRange.Remove(npc);
                UpdateClosestNPC();
                PeopleMovement movementComponent = npc.GetComponent<PeopleMovement>();
                if (movementComponent != null)
                {
                    movementComponent.InteractIndicator.SetActive(false);
                }
            }
        }
    }


    private void UpdateClosestNPC()
    {
        if (npcsInRange.Count > 0)
        {
            closestNPC = npcsInRange[0];  // Assuming the first one in range is the closest
            ClosestNpcPF = closestNPC.GetComponent<PeopleMovement>();
        }
        else
        {
            closestNPC = null;
        }
    }




    public void StartInteraction()
    {
        if (TypingManager.isInteracting) return;

        ClosestNpcPF.InteractIndicator.SetActive(false);
        StartCoroutine(WaitBeforeTyping());

        // Tetap freeze posisi X, Z, dan rotasi selama interaksi
        ClosestNpcPF.RB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        ClosestNpcPF.RB.velocity = Vector3.zero;

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.StartAttack();
            playerController.enabled = false;
            playerController.canMove = false;
        }

        if (ClosestNpcPF != null)
        {
            ClosestNpcPF.StopMovement();
            ClosestNpcPF.enabled = false;
        }

        ClosestNpcPF.animator.SetBool("doAction", true);
        SfxAudioClip.Instance.PlayRandomAudio(SfxAudioClip.AudioCategory.NPC);
        Debug.Log("Interaction started");
    }

    public void EndInteraction()
    {
        StartCoroutine(TypingManager.ResetCameraZoom());

        if (TypingManager.isInteracting)
        {
            if (ClosestNpcPF != null && ClosestNpcPF.RB != null)
            {
                ClosestNpcPF.RB.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }

            if (playerController != null)
            {
                playerController.StopAttack();
                playerController.enabled = true;
                
            }

            if (ClosestNpcPF != null)
            {
                ClosestNpcPF.enabled = true;
                ClosestNpcPF.ResumeMovement();
            }

            StartCoroutine(WaitBeforeEnd());

            GameObject npcToDestroy = closestNPC;
            closestNPC = null; // Kosongkan referensi untuk menghindari akses ke objek yang dihapus
            StartCoroutine(DestroyNPC(npcToDestroy));
            TypingManager.isInteracting = false;

        }
    }


    IEnumerator WaitBeforeTyping()
    {
        yield return new WaitForSeconds(1f);
        TypingManager.getRandomWord();
        TypingManager.isInteracting = true;


    }

    IEnumerator WaitBeforeEnd()
    {
        yield return new WaitForSeconds(.5f);

        if (playerController != null)
        {
            playerController.canMove = true;

        }

    }

    IEnumerator DestroyNPC(GameObject NPCtoDestroy)
    {
        yield return new WaitForSeconds(2f);

        ClosestNpcPF.animator.SetTrigger("fading");

        yield return new WaitForSeconds(1f);

        // Cek kembali jika NPC masih ada sebelum menghancurkannya
        if (NPCtoDestroy != null)
        {
            if (spawnManager != null) spawnManager.curSpawns--;
            npcsInRange.Remove(NPCtoDestroy);
            Destroy(NPCtoDestroy);
        }
    }
}
