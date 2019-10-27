using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{

    ParticleSystem ps;

    public float diveScale;
    public int diveSpeed;
    public int diveRate;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        var sz = ps.sizeOverLifetime;
        var main = ps.main;
        var emission = ps.emission;
        if (Input.GetButton("Dive"))
        {
            sz.zMultiplier = diveScale;
            main.startSpeed = diveSpeed;
            emission.rateOverTime = diveRate;
        }
        else
        {
            sz.zMultiplier = 1f;
            main.startSpeed = 40;
            emission.rateOverTime = 40;
        }
    }
}
