using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSequence : MonoBehaviour
{
    public float fallDuration = 30f;

    [Header("Walls")]
    public Material walls;
    public float initSpeed = 0.01f;
    public float termSpeed = 0.02f;
    public float deltaSpeed = 0.001f;
    float speed;

    [Header("Floor")]
    public Transform floor;
    public Material floorMat;
    public float initFloorScale = 10f;
    public float termFloorScale = 2f;
    public float deltaFloorScale = 0.01f;
    public float floorHeightMax = 10f;
    public float foorHeightDelta = 0.5f;

    private void Start()
    {
        walls.mainTextureOffset = Vector2.zero;
        speed = initSpeed;
        StartCoroutine(SpeedUp());
        StartCoroutine(Duration());
    }

    // Update is called once per frame
    void Update()
    {
        walls.mainTextureOffset = new Vector2(0, walls.mainTextureOffset.y - speed);
    }

    IEnumerator SpeedUp()
    {
        while(speed < termSpeed)
        {
            speed += deltaSpeed;
            yield return new WaitForEndOfFrame();
        }
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
                floor.Translate(Vector3.up * foorHeightDelta, Space.World);

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
