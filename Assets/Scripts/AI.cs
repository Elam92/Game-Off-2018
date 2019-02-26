using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI
{
    protected List<AIShip> ownShips;
    protected List<Ship> targetShips;

    public abstract void Initialize();
    public abstract void DoActions();

    public List<AIShip> GetShips()
    {
        return ownShips;
    }

    public List<Ship> GetTargetShips()
    {
        return targetShips;
    }
}
