using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
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
            _rigidbody.AddForce(new Vector2(speed, 0f) * Time.deltaTime);
        }
        else if (Input.GetKey("a"))
        {
            _rigidbody.AddForce(new Vector2(0f, speed) * Time.deltaTime);
        }
    }
}
