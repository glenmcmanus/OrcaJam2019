using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSequence : MonoBehaviour
{
    public static FallSequence instance;

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

    private void Start()
    {
        walls.mainTextureOffset = Vector2.zero;
        hasKey.hasKey = 0;
        //StartCoroutine(Duration());
        StartCoroutine(Progress());
    }

    IEnumerator Progress()
    {
        for (; ; )
        {
            walls.mainTextureOffset = new Vector2(0, walls.mainTextureOffset.y -
                                       fallSpeed[(int)FallingPlayer.instance.fallstate]);

            curProgress += progress[(int)FallingPlayer.instance.fallstate];

            if (curProgress >= fallLength + 180)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        for(int i = 0; i < 60; i++)
        {
            Camera.main.transform.Rotate(new Vector3(-1.3f, 0, 0));
            Camera.main.transform.Translate(Vector3.down * 0.5f, Space.World);
            Camera.main.transform.Translate(Vector3.back * 0.35f, Space.World);
            yield return new WaitForEndOfFrame();
        }

        Door.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Door.instance.transform.localPosition = new Vector3(0, PieceDropper.instance.doorHeight, 8);

        FallingPlayer.instance.enabled = false;

        FallingPlayer.instance.fallstate = FallState.Drag;
        FallingPlayer.instance.transform.position = new Vector3(FallingPlayer.instance.transform.position.x,
                                                                FallingPlayer.instance.dragPosY,
                                                                0);

        FallingPlayer.instance.collider.enabled = true;

        Camera.main.transform.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForFixedUpdate();

        FallingPlayer.instance.platformPlayer.enabled = true;

        FallingPlayer.instance.animator.SetBool("phase2", true);

        yield return new WaitForEndOfFrame();

        FallingPlayer.instance.GetComponent<Rigidbody>().useGravity = true;

        while (FallingPlayer.instance.platformPlayer.CanJump() == false)
            yield return null;

        FallingPlayer.instance.animator.SetBool("grounded", true);

        PieceDropper.instance.SwitchPhases();

        FallingPlayer.instance.platformPlayer.StartCoroutine(FallingPlayer.instance.platformPlayer.InputHandler());
    }
}
