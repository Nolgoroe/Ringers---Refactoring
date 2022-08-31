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
}

/// create a context menu method that will go to resources folder and retrieve all BG's into
/// the BG Paths array