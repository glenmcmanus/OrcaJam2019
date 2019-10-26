using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {
    // Start is called before the first frame update
    public PieceDB pieces;
    public static Vector3 scale;
    private float x;
    private float z;

    public float minInterval;
    public Vector3 initVelocity = new Vector3(0, 1, 0);

    public float radioSpawnTime = 30f;
    public float bedSpawnTime = 30f;


    void Start() {
        scale = transform.localScale;
        StartCoroutine(Spawn());
        x = transform.localScale.x;
        z = transform.localScale.z;

        StartCoroutine(SpecialDrop());
    }

    IEnumerator SpecialDrop()
    {
        yield return new WaitForSeconds(radioSpawnTime);

        //spawn radio

        yield return new WaitForSeconds(bedSpawnTime);

        //spawn bed
    }

    IEnumerator Spawn() {
         for(;;) {
             spawnObject();// execute block of code here
             yield return new WaitForSeconds(minInterval);
         }
     }

     void spawnObject(){

        float xr = Random.Range(0.0f, 1.0f);
        float zr = Random.Range(0.0f, 1.0f);
        Vector3 randomPosition = new Vector3(transform.position.x + xr * x, transform.position.y, transform.position.z + zr * z);
        print(randomPosition);
        int r = Random.Range(0, 6);
        Piece go = Instantiate(pieces.piece[r], randomPosition, Quaternion.identity);
        go.id = r;
        go.GetComponent<Rigidbody>().AddForce(initVelocity);
    }

}