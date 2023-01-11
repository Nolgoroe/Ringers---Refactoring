using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cluster", menuName = "ScriptableObjects/Create Cluster")]
public class ClusterSO : ScriptableObject
{
    public LevelSO[] clusterLevels;
    public List<LootTables> clusterLootTables;
    public GameObject clusterPrefabToSummon;
}
