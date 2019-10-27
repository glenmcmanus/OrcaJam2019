using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSequence : MonoBehaviour
{
    public int fallLength = 30;
    public int[] progress = new int[] { 1, 2 };
    public int curProgress = 0;

    [Header("Walls")]
    public Material walls;
    public float[] fallSpeed = new float[2] { 4f, 8f };
    float speed;

    [Header("Floor")]
    public Transform floor;
    public Material floorMat;
    public float initFloorScale = 10f;
    public float termFloorScale = 2f;
    public float deltaFloorScale = 0.01f;
    public float floorHeightMax = 10f;
    public float floorHeightDelta = 0.5f;

    private void Start()
    {
        walls.mainTextureOffset = Vector2.zero;
        StartCoroutine(Progress());
    }

    IEnumerator Progress()
    {
        for (; ; )
        {
            walls.mainTextureOffset = new Vector2(0, walls.mainTextureOffset.y -
                                       fallSpeed[(int)FallingPlayer.instance.fallstate]);

            curProgress += progress[(int)FallingPlayer.instance.fallstate];

            if (curProgress >= fallLength)
            {
                SpawnScript.instance.StopCoroutine(SpawnScript.instance.spawn);
                break;
                //UnityEngine.SceneManagement.SceneManager.LoadScene(1,
                //                        UnityEngine.SceneManagement.LoadSceneMode.Single);
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
