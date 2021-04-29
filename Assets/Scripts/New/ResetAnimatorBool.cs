using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [System.Serializable]
    public struct AnimatorBool
    {
        public string name;
        public bool state;
    }

    public AnimatorBool[] targetBools;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var targetBool in targetBools)
        {
            if (targetBool.name == null || targetBool.name.Length == 0)
            {
                throw new System.ArgumentNullException();
            }
            animator.SetBool(targetBool.name, targetBool.state);
        }
    }
}
