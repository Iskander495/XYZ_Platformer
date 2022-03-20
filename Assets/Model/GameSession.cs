using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _data;

    public PlayerData Data => _data;
}
