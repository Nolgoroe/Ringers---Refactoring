using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldEnum
{
    WalnutGrove,
    VinebloomPines,
    GreyMossBog,
    RedRootCovert,
    ThornberryWoods
}

public class ZoneManager : MonoBehaviour
{
    public string[] BGPaths;

    public Dictionary<WorldEnum, string> BGEnumToResource;

    private void Start()
    {
        BGEnumToResource = new Dictionary<WorldEnum, string>();

        for (int i = 0; i < BGPaths.Length; i++)
        {
            BGEnumToResource.Add((WorldEnum)i, BGPaths[i]);
        }
    }
}


/// create a context menu method that will go to resources folder and retrieve all BG's into
/// the BG Paths array