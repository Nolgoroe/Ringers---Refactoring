using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Linq;

public class LevelMapCustomButton : CustomButtonParent
{
    [SerializeField] private LevelSO connectedLevelSO;
    [SerializeField] private ClusterSO connectedCluster;

    [SerializeField] private int indexInCluster;
    public override void OnClickButton()
    {
        buttonEventsInspector?.Invoke();
    }

    //called from button
    public void ActionsOnClickLevel ()
    {
        GameManager.instance.ClickOnLevelIconMapSetData(connectedLevelSO, connectedCluster, indexInCluster);
        UIManager.instance.DisplayLaunchLevelPopUp(connectedLevelSO);
    }

    public override void OverrideSetMyElement(string[] texts, Sprite[] sprites, System.Action[] actions = null)
    {
        base.SetMyElement(texts, sprites);
    }

    [ContextMenu("Populate cluster SO")]
    public void PopulateCluster()
    {
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        foreach (var cluster in gm.allClusters)
        {
            for (int i = 0; i < cluster.clusterLevels.Length; i++)
            {
                if(cluster.clusterLevels[i] == connectedLevelSO)
                {
                    connectedCluster = cluster;
                    indexInCluster = i;
                }
            }
        }
    }
}
