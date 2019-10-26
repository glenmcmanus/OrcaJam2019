using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject [] pieceArray;
    public static Vector3 scale;
    private float x;
    private float z;

    public float minInterval;

    void Start() {
        scale = transform.localScale;
        StartCoroutine("DoCheck");
        x = transform.localScale.x;
        z = transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DoCheck() {
         for(;;) {
             spawnObject();// execute block of code here
             yield return new WaitForSeconds(minInterval);
         }
     }

     void spawnObject(){
             float xr = Random.Range(0.0f,1.0f);
             float zr = Random.Range(0.0f,1.0f);
             Vector3 randomPosition = new Vector3 (transform.position.x + xr*x, transform.position.y, transform.position.z + zr*z );
             print(randomPosition);
             int r = Random.Range(0,6);
             Instantiate(pieceArray[r], randomPosition, Quaternion.identity);
     }

}