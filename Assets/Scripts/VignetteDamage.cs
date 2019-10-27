using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VignetteDamage : MonoBehaviour
{

    //public bool ouch;

    public PostProcessProfile profile;
    public float duration = 0.5f;
    Vignette m_Vignette;
    float hitTime;


    void Start()
    {
        m_Vignette = profile.GetSetting<Vignette>();
        m_Vignette.color.value = new Color(0.5f, 0f, 0f, 1f);
        //profile = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, m_Vignette);
        hitTime = Time.time - 2*Mathf.PI;

    }

    /*void Update()
    {
        if(Time.time - hitTime < Mathf.PI-.5f)
        {
            m_Vignette.intensity.value = Mathf.Max(Mathf.Sin((Time.time - hitTime + 0.5f) * 2) - 0.5f, 0f);
        }
    }*/

    public void Ouch()
    {
        StartCoroutine(Strobe());
       // hitTime = Time.time;
    }

    IEnumerator Strobe()
    {
        float timer = Time.time + duration;
        while(Time.time < timer)
        {
            m_Vignette.intensity.value = 1 - (timer - Time.time) / duration;
            yield return null;
        }

        m_Vignette.intensity.value = 1;

        timer = Time.time + duration;
        while (Time.time < timer)
        {
            m_Vignette.intensity.value = (timer - Time.time) / duration;
            yield return null;
        }

        m_Vignette.intensity.value = 0;
    }

    /*// Start is called before the first frame update
    public PostProcessVolume pVolume;
    Vignette v;

    void Start()
    {
        v = ScriptableObject.CreateInstance<Vignette>();
        pVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 100f, v);
    }

    // Update is called once per frame
    void Update()
    {
        v.intensity.value = Mathf.Sin(Time.time);
    }*/
}
