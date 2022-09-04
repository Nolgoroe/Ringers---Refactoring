using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Action", menuName = "ScriptableObjects/Create Level Action")]
public class LevelActionsSO : ScriptableObject
{
    // in the future we'll create different Scriptable objects for different types of actions - such as GFXLevelActionSO and stuff like that.
    // this list holds functions relavent to it's type and will be used in the "Main level SO" to create the "built action lists of before, during and after actions".
}
