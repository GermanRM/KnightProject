using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerCollider : MonoBehaviour
{
    [Header("References")]
    public GameObject passGO;
    public GameObject doorColliderGO;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            passGO.SetActive(true);
            doorColliderGO.SetActive(true);
        }
    }
}
