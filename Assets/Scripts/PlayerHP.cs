using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new PlayerHP", menuName = "PlayerHP")]
public class PlayerHP : ScriptableObject
{
    public int maxHP = 3;
    public int curHP = 3;
}
