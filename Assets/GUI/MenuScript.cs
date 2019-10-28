using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    public GameObject buttons;
    public GameObject howToPlayObject1;
    public GameObject howToPlayObject2;

    public void PressStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("FallingTest",
                 UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
    public void PressHowToPlay()
    {
        buttons.SetActive(false);
        howToPlayObject1.SetActive(true);
    }

    public void PressNext()
    {
        howToPlayObject1.SetActive(false);
        howToPlayObject2.SetActive(true);
    }
    public void PressReturn()
    {
        buttons.SetActive(true);
        howToPlayObject2.SetActive(false);
    }
}
