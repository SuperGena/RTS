using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWariorBuilding : Building
{

    public override void Update()
    {
        base.Update();
    }

    public override void ShowInfo()
    {
        //base.ShowInfo();
        GameManager.ActivateUI(0);
    }

    public override void AddWariorToQueue()
    {
        base.AddWariorToQueue();
    }

    public override void LevelUP()
    {
        base.LevelUP();
    }
}
