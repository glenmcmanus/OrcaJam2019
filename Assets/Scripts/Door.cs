using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door instance;

    public HasKey hasKey;
    public int keyCount;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasKey.hasKey == keyCount)
        {
            Debug.Log("You win!");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Victory", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
