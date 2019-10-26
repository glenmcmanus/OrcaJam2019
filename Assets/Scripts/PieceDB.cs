using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PieceDB", menuName = "Piece/Piece DB")]
public class PieceDB : ScriptableObject
{
    public Piece[] piece;
}
