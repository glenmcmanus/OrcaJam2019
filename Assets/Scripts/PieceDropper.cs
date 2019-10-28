using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PieceDropper : MonoBehaviour
{
    public static PieceDropper instance;

    public int debugPool = 0;

    public PieceDB pieces;
    public List<PieceSpawn> spawnQueue = new List<PieceSpawn>();

    public float doorHeight = 10f;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        if(debugPool > 0)
        {
            for(int i = 0; i < debugPool; i++)
            {
                spawnQueue.Add(new PieceSpawn(Random.Range(0, 6), Random.Range(-4, 4), Vector3.zero));
            }
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Drop());
        }
    }

    /// <summary>
    /// Phase 1 is over, don't add more pieces, start dropping them for phase 2
    /// </summary>
    public void SwitchPhases()
    {
        GetComponent<BoxCollider>().enabled = false;
        FindObjectOfType<Door>().GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(Drop());
    }

    public void DespawnPiece(Piece piece)
    {
        spawnQueue.Add(new PieceSpawn(piece.id, 
                                      piece.gameObject.transform.position.x,
                                      piece.transform.rotation.eulerAngles));
        Destroy(piece.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Piece")
        {
            Piece p;
            if (!other.TryGetComponent<Piece>(out p))
                return;
            
            if(p)
                DespawnPiece(other.GetComponent<Piece>());
            else
                DespawnPiece(other.GetComponentInParent<Piece>());
        }
    }

    IEnumerator Drop()
    {
        while (spawnQueue.Count > 0)
        {
            int index = spawnQueue.Count - 1;
            Vector3 pos = new Vector3(spawnQueue[index].xpos, transform.position.y,
                                               FallingPlayer.instance.transform.position.z);

            Piece p = Instantiate(pieces.piece[spawnQueue[index].id], pos,
                                   Quaternion.Euler(spawnQueue[index].rotation), transform );

            Rigidbody rb = p.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY 
                             | RigidbodyConstraints.FreezePositionZ;
            spawnQueue.RemoveAt(index);

            yield return new WaitForSeconds(2.5f);
        }
    }
}

[System.Serializable]
public struct PieceSpawn
{
    public int id;
    public float xpos;
    public Vector3 rotation;

    public PieceSpawn(int id, float xpos, Vector3 rotation)
    {
        this.id = id;
        this.xpos = xpos;
        this.rotation = rotation;
    }
}
