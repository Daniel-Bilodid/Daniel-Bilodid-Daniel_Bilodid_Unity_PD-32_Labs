using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int _chest;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.gameObject.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.OpenChest(_chest);
            Destroy(gameObject);
        }
    }
}
