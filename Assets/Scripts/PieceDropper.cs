using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PieceDropper : MonoBehaviour
{
    public PieceDB pieces;
    public List<PieceSpawn> spawnQueue = new List<PieceSpawn>();
    /// <summary>
    /// Phase 1 is over, don't add more pieces, start dropping them for phase 2
    /// </summary>
    public void SwitchPhases()
    {
        GetComponent<BoxCollider>().enabled = false;
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

            yield return new WaitForSeconds(1);
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
