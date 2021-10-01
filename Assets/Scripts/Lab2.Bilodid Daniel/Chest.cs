using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int _coinsAmount;

    public bool Activated { private get; set; }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Activated)
        {
            return;
        }
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            Debug.Log($"CoinsAdded{_coinsAmount}");
            player.CoinsAmount += _coinsAmount;
            Destroy(gameObject);
        }
    }
}
