using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePosition : MonoBehaviour
{
    public Rigidbody2D _rigidbody;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("d"))
        {
            _rigidbody.MovePosition(_rigidbody.position + new Vector2 (speed, 0f) * Time.deltaTime);
        }
      
    }
}
