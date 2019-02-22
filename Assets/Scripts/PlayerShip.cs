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
            if (BattleController.SelectedShip != null && !BattleController.SelectedShip.Equals(this))
            {
                //GameGrid.selectedNode.ResetNeighbours();
                //GameGrid.UpdateNodeStates(GameGrid.selectedNode.);
            }

            if (!active)
            {
                GameGrid.selectedNode = GetCurrentNode();
                stateMachine.Transition(ShipStateInputs.Selected);
                active = true;
            }
        }
    }
}
