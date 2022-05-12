using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Spells/Healing Spell")]
public class HealingSpell : SpellItem
{
    public int healAmount;

    public override void AttemptCastSpell(AnimatorHandler animatorHandler, PlayerStats stats)
    {
        base.AttemptCastSpell(animatorHandler, stats);
        if (spellWarmUpFX != null)
        {
            GameObject instanceWarmUpFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            ParticleSystem particleSystem = instanceWarmUpFX.GetComponent<ParticleSystem>();
            Destroy(instanceWarmUpFX, particleSystem != null ? particleSystem.main.duration : 5f);
        }
        animatorHandler.PlayTargetAnimation(spellAnimation, true);
    }

    public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats stats)
    {
        base.SuccessfullyCastSpell(animatorHandler, stats);
        if (spellCastFX != null)
        {
            GameObject instanceCastFX = Instantiate(spellCastFX, animatorHandler.transform);
            ParticleSystem particleSystem = instanceCastFX.GetComponent<ParticleSystem>();
            Destroy(instanceCastFX, particleSystem != null ? particleSystem.main.duration : 5f);
        }
        stats.Heal(healAmount);
    }
}
