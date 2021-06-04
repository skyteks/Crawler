using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);

        PickUpWeapon(playerManager);
    }

    private void PickUpWeapon(PlayerManager playerManager)
    {
        PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
        PlayerLocomotion playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
        PlayerAnimatorHandler animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();

        playerLocomotion.rigid.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation(PlayerAnimatorHandler.hashPickUpItem, true);
        playerInventory.weaponsInventory.Add(weapon);

        playerManager.itemInteractableGO.SetActive(true);
        playerManager.itemInteractableGO.GetComponentInChildren<UnityEngine.UI.Text>().text = weapon.name;

        Destroy(gameObject);
    }
}
