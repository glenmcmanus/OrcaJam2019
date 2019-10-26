using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PieceDropper : MonoBehaviour
{
    public int debugPool = 0;

    public PieceDB pieces;
    public List<PieceSpawn> spawnQueue = new List<PieceSpawn>();

    private void Awake()
    {
        if(debugPool > 0)
        {
            for(int i = 0; i < debugPool; i++)
            {
                spawnQueue.Add(new PieceSpawn(Random.Range(0, pieces.piece.Length), Random.Range(-4, 4)));
            }
            SwitchPhases();
        }
    }

    /// <summary>
    /// Phase 1 is over, don't add more pieces, start dropping them for phase 2
    /// </summary>
    public void SwitchPhases()
    {
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(Drop());
    }

    public void DespawnPiece(Piece piece)
    {
        spawnQueue.Add(new PieceSpawn(piece.id, piece.gameObject.transform.position.x));
        Destroy(piece.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Piece")
        {
            DespawnPiece(other.GetComponent<Piece>());
        }
    }

    IEnumerator Drop()
    {
        while(spawnQueue.Count > 0)
        {
            Piece p = Instantiate(pieces.piece[spawnQueue[0].id], transform);

            p.transform.position = new Vector3(spawnQueue[0].xpos, transform.position.y, transform.position.z);
            p.GetComponent<Rigidbody>().useGravity = true;
            spawnQueue.RemoveAt(0);

            yield return new WaitForSeconds(2.5f);
        }
    }
}

[System.Serializable]
public struct PieceSpawn
{
    public int id;
    public float xpos;

    public PieceSpawn(int id, float xpos)
    {
        this.id = id;
        this.xpos = xpos;
    }
}
