using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    public HasKey hasKey;

    private void OnTriggerEnter(Collider other)
    {
        hasKey.hasKey = 1;
        Destroy(transform.parent.gameObject);
    }
}
