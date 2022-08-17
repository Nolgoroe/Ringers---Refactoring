using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("In game Data")]
    public Transform gameBoard;
    public LevelSO currentLevel;
    void Start()
    {
        instance = this;
    }

}
