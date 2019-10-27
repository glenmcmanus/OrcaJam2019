using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSequence : MonoBehaviour
{
    public int fallLength = 30;
    public int[] progress = new int[] { 1, 2 };
    public int curProgress = 0;
    public HasKey hasKey;

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
        hasKey.hasKey = 0;
        //StartCoroutine(Duration());
    }

    // Update is called once per frame
    void Update()
    {
        walls.mainTextureOffset = new Vector2(0, walls.mainTextureOffset.y -
                                       fallSpeed[(int)FallingPlayer.instance.fallstate]);

        curProgress += progress[(int)FallingPlayer.instance.fallstate];

        if(curProgress >= fallLength)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1, 
                                    UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }

    IEnumerator Duration()
    {
        floorMat.mainTextureScale = initFloorScale * Vector2.one;

        float end = Time.time + fallLength;
        while(floorMat.mainTextureScale.x > termFloorScale)
        {
            floorMat.mainTextureScale -= deltaFloorScale * Vector2.one;
            floorMat.mainTextureOffset -= deltaFloorScale * Vector2.one;

            if(floor.position.y < floorHeightMax)
                floor.Translate(Vector3.up * floorHeightDelta, Space.World);

            yield return new WaitForEndOfFrame();
        }

        floorMat.mainTextureScale = termFloorScale * Vector2.one;
        floor.position = new Vector3(0, floorHeightMax, 0);
    }
}
