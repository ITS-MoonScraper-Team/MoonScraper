using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerLimitCollider : MonoBehaviour
{
    public BoxCollider2D collider;

    void Start()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();
    }

}
