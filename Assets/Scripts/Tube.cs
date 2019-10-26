using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public float fallDuration = 30f;

    [Header("Speed")]
    public float initSpeed = 0.01f;
    public float termSpeed = 0.02f;
    public float deltaSpeed = 0.001f;
    float speed;

    [Header("Materials")]
    public Material walls;
    public Material floor;

    private void Start()
    {
        walls.mainTextureOffset = Vector2.zero;
        speed = initSpeed;
        StartCoroutine(SpeedUp());
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
}
