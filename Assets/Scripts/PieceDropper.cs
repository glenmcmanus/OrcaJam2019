﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PieceDropper : MonoBehaviour
{
    public static PieceDropper instance;

    public int debugPool = 0;

    public PieceDB pieces;
    public List<PieceSpawn> spawnQueue = new List<PieceSpawn>();

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
            Piece p = other.GetComponent<Piece>();
            if(p)
                DespawnPiece(other.GetComponent<Piece>());
            else
                DespawnPiece(other.GetComponentInParent<Piece>());
        }
    }

    IEnumerator Drop()
    {
        while(spawnQueue.Count > 0)
        {
            Piece p = Instantiate(pieces.piece[spawnQueue[0].id], transform);

            p.transform.position = new Vector3(spawnQueue[0].xpos, transform.position.y, transform.position.z);
            Rigidbody rb = p.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY 
                             | RigidbodyConstraints.FreezePositionZ;
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
