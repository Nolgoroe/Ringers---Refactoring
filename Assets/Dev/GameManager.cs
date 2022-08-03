using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("In game Data")]
    public Transform gameBoard;

    void Start()
    {
        instance = this;
    }

}
