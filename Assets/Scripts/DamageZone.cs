using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D other)
    {
        PenguinController controller = other.gameObject.GetComponent<PenguinController>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}