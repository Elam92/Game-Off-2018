using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShip : Ship, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(transform.name);
        Debug.Log(BattleController.IsPlayerTurn());
        if (BattleController.IsPlayerTurn() && !turnFinished)
        {
            if (!active && BattleController.SelectedShip == null)
            {
                stateMachine.Transition(ShipStateInputs.Selected);
                active = true;
            }
        }
    }
}
