using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PieceDropper : MonoBehaviour
{
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
        spawnQueue.Add(new PieceSpawn(piece.id, new Vector2(piece.gameObject.transform.position.x, Time.time)));
        Destroy(piece.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Piece")
        {
            DespawnPiece(other.GetComponent<Piece>());
        }
    }
}

public struct PieceSpawn
{
    public int id;
    public Vector2 pos;

    public PieceSpawn(int id, Vector2 pos)
    {
        this.id = id;
        this.pos = pos;
    }
}
