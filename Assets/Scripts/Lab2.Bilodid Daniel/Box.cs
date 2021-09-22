using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private int box;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.gameObject.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.OpenBox(box);
            Destroy(gameObject);
        }
    }
}
