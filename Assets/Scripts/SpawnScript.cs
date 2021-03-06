﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    public static SpawnScript instance;

    // Start is called before the first frame update
    public PieceDB pieces;
    public float x = 4;
    public float z = 4;
    public LayerMask pieceLayer;

    public float minInterval;
    public float initVelocity = 6;

    public Vector2 keySpawnTime = new Vector2(4f, 8f);
    public float radioSpawnTime = 30f;

    public Coroutine spawn;

    private float xr;
    private float zr;
    private Vector3 randomPosition;
    private Piece go;
    private int r;

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

    void Start() {
        spawn = StartCoroutine(Spawn());
        StartCoroutine(SpecialDrop());
        StartCoroutine(KeySpawn());
    }

    IEnumerator SpecialDrop()
    {
        yield return new WaitForSeconds(radioSpawnTime);
        xr = Random.Range(-1.0f, 1.0f);
        zr = Random.Range(-1.0f, 1.0f);
        randomPosition = new Vector3(transform.position.x + xr * x, transform.position.y, transform.position.z + zr * z);
        go = Instantiate(pieces.piece[6],randomPosition, Quaternion.identity);
        go.transform.SetParent(transform, true);
        go.id = 6;
        go.GetComponent<Rigidbody>().velocity = Vector3.up * initVelocity * 0.1f;
        go.GetComponent<Rigidbody>().AddTorque(Vector3.right * 8);
        //spawn radio

        while (FallSequence.instance.curProgress < FallSequence.instance.fallLength - 120)
            yield return null;

        StopCoroutine(spawn);

        while (FallSequence.instance.curProgress < FallSequence.instance.fallLength)
            yield return null;


        xr = Random.Range(-1.0f, 1.0f);
        zr = Random.Range(-1.0f, 1.0f);
        randomPosition = new Vector3(0, transform.position.y, 9f);
        Piece door = Instantiate(pieces.piece[7], randomPosition, Quaternion.Euler(270,0,0));
        door.transform.SetParent(transform, true);
        door.id = 7;
        door.GetComponent<Rigidbody>().velocity = FallingPlayer.instance.pieceDragSpd * Vector3.up;

    }

    IEnumerator KeySpawn()
    {
        float timer;
        for (int i = 0; i < pieces.piece[7].GetComponent<Door>().keyCount; i++)
        {
            timer = Random.Range(keySpawnTime.x, keySpawnTime.y);
            yield return new WaitForSeconds(timer);
            xr = Random.Range(-1.0f, 1.0f);
            zr = Random.Range(-1.0f, 1.0f);
            randomPosition = new Vector3(transform.position.x + xr * x, transform.position.y, transform.position.z + zr * z);

            while(Physics.OverlapBox(randomPosition, Vector3.one * 0.5f, 
                                     Quaternion.identity, pieceLayer).Length > 0)
            {
                xr = Random.Range(-1.0f, 1.0f);
                zr = Random.Range(-1.0f, 1.0f);
                randomPosition = new Vector3(transform.position.x + xr * x, transform.position.y, transform.position.z + zr * z);
                yield return null;
            }

            Piece key = Instantiate(pieces.piece[8], randomPosition, Quaternion.identity);
            key.id = 8;
            key.GetComponent<Rigidbody>().velocity = FallingPlayer.instance.pieceDragSpd * Vector3.up;
        }
    }

    IEnumerator Spawn() {
         for(;;) {
             spawnObject();// execute block of code here
             yield return new WaitForSeconds(minInterval);
         }
     }

     void spawnObject(){
        xr = Random.Range(-1.0f, 1.0f);
        zr = Random.Range(-1.0f, 1.0f);
        randomPosition = new Vector3(transform.position.x + xr * x, transform.position.y, transform.position.z + zr * z);
        r = Random.Range(0, 6);

        int rot = Random.Range(0, 4);
        Vector3 rotation = Vector3.zero;

        if (rot == 1)
        {
            rotation = new Vector3(1500, 0, 0);
        }
        else if(rot == 2)
        {
            rotation = new Vector3(0, 1500, 0);
        }
        else if(rot == 3)
        {
            rotation = new Vector3(0, 0, 1500);
        }

        Piece go = Instantiate(pieces.piece[r], randomPosition, Quaternion.identity);
        go.transform.SetParent(transform, true);
        go.id = r;
        go.GetComponent<Rigidbody>().velocity = Vector3.up * FallingPlayer.instance.pieceDragSpd;
        go.GetComponent<Rigidbody>().AddTorque(rotation);
    }

}