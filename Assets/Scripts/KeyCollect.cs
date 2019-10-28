using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    public HasKey hasKey;

    private void OnTriggerEnter(Collider other)
    {
        FallingPlayer.instance.aSource.PlayOneShot(FallingPlayer.instance.cheerSFX);
        hasKey.hasKey += 1;
        Destroy(transform.parent.gameObject);
    }
}
