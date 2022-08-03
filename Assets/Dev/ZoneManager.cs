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
