using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotion : MonoBehaviour
{
    [SerializeField] private int ManaP;
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.gameObject.GetComponent<PlayerMover>();
        if (player != null)
        {
            player.AddMana( ManaP);
            Destroy(gameObject);
        }
    }
}
