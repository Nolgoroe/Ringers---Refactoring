using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWorkshopCustomWindow :  BasicCustomUIWindow
{
    [SerializeField] Transform materialsContent;

    private void OnEnable()
    {
        SortWorkshop(0);
    }

    private void SortWorkshop(int index)
    {

    }
}
