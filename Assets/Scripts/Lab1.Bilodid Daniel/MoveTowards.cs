using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowards : MonoBehaviour
{
    [SerializeField]private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("d"))
        {
            transform.position = Vector2.MoveTowards (transform.position, new Vector2(speed, transform.position.y), speed * Time.deltaTime);
        }
    }
}
