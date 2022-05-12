using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellItem : Item
{
    public enum SpellTypes
    {
        Pyro,
    }

    [Header("Stamina Costs")]
    public int manaCost;

    [Header("Spell Effects")]
    public GameObject spellWarmUpFX;
    public GameObject spellCastFX;

    [Header("Spell Info")]
    public SpellTypes spellType;
    [TextArea]
    public string description;

    [Header("Spell Animations")]
    public string spellAnimation;

    public virtual void AttemptCastSpell(AnimatorHandler animatorHandler, PlayerStats stats)
    {
    }

    public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats stats)
    {
        stats.TakeMana(manaCost);
    }
}
