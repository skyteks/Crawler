using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BackStabTrigger : MonoBehaviour
{
    public Transform backStabberStandpoint;
    private BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponentInChildren<BoxCollider>();
    }
}
