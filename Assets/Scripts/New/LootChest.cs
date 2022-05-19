using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootChest : Interactable
{
    public Transform playerStandpoint;
    public GameObject itemSpawner;
    public WeaponItem itemInChest { get; set; }

    private Animator anim;
    private static int hashChestOpen = Animator.StringToHash("Chest Open");

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public override void Interact(PlayerManager playerManager)
    {
        Vector3 rotationDirection = transform.position - playerManager.transform.position;
        rotationDirection.y = 0f;
        rotationDirection.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
        targetRotation = Quaternion.Slerp(playerManager.transform.rotation, targetRotation, Time.deltaTime * 100f);
        playerManager.transform.rotation = targetRotation;

        playerManager.OpenChestInteraction(playerStandpoint);
        anim?.Play(hashChestOpen);
        StartCoroutine(SpawnItemInChest());

        WeaponPickup pickup = itemSpawner.GetComponent<WeaponPickup>();
        if (pickup != null)
        {
            pickup.weapon = itemInChest;
        }

    }

    IEnumerator SpawnItemInChest()
    {
        yield return Yielders.Get(1f);
        Instantiate(itemSpawner, transform.position + transform.up * 0.5f, transform.rotation);
        Destroy(this);
    }
}
