using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaEater : MonoBehaviour
{
    [SerializeField] private int _ManaEater;
    [SerializeField] private float _ManaEaterDelay;
    private float _lastManaEaterTime;
    private PlayerMover _player;


    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player != null)
            _player = player;


    }
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerMover player = other.GetComponent<PlayerMover>();
        if (player == _player)
            _player = null;
    }

    private void Update()
    {
        if (_player != null && Time.time - _lastManaEaterTime > _ManaEaterDelay)
        {
            _lastManaEaterTime = Time.time;
            _player.TakeManaEater(_ManaEater);
        }
    }
}


