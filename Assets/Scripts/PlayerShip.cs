﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShip : Ship, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(transform.name);
        if (BattleController.Instance.IsPlayerTurn() && !turnFinished)
        {
            if (!active && BattleController.Instance.SelectedShip == null)
            {
                stateMachine.Transition(ShipStateInputs.Selected);
            }
        }
    }
}