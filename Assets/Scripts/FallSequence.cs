using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSequence : MonoBehaviour
{
    public float fallDuration = 30f;

    [Header("Walls")]
    public Material walls;
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
        //StartCoroutine(Duration());
    }

    // Update is called once per frame
    void Update()
    {
        walls.mainTextureOffset = new Vector2(0, walls.mainTextureOffset.y - 
                                       FallingPlayer.instance.fallSpeed[(int)FallingPlayer.instance.fallstate]);
    }

    IEnumerator Duration()
    {
        floorMat.mainTextureScale = initFloorScale * Vector2.one;

        float end = Time.time + fallDuration;
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

        while(Time.time < end)
        {
            yield return null;
        }

        //do phase change
    }
}
