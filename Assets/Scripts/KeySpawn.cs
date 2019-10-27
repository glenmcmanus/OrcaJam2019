using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawn : MonoBehaviour
{

    public float startTorque = 5f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddTorque(new Vector3(Random.Range(-startTorque, startTorque), Random.Range(-startTorque, startTorque), Random.Range(-startTorque, startTorque)));
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/
}
