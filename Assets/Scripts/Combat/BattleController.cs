using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public Transform playerShipContainer;
    public Transform aiShipContainer;

    private StateMachine<BattleStateInputs> stateMachine;
    private List<Ship> playerShips;
    private List<AIShip> aiShips;
    private List<Ship> usedShips;

    private static Ship selectedShip;
    public static Ship SelectedShip
    {
        get
        {
            return selectedShip;
        }
        set
        {

            selectedShip = value;
        }
    }

    private static bool isPlayerTurn;

    // Use this for initialization
    private void Start()
    {
        usedShips = new List<Ship>();
        selectedShip = null;

        // Add Ships in Scene
        playerShips = new List<Ship>();
        foreach(Transform ship in playerShipContainer)
        {
            playerShips.Add(ship.GetComponent<Ship>());
        }

        aiShips = new List<AIShip>();
        foreach(Transform aiShip in aiShipContainer)
        {
            aiShips.Add(aiShip.GetComponent<AIShip>());
        }

        var finishState = new FinishBattleState(this);
        var aiTurnState = new AITurnState(this, aiShips, finishState);
        var playerTurnState = new PlayerTurnState(this, playerShips, finishState);
        var initializeState = new InitializeBattleState(this, playerShips, playerTurnState);

        aiTurnState.AddTransition(BattleStateInputs.PlayerTurn, playerTurnState);
        aiTurnState.AddTransition(BattleStateInputs.Finish, finishState);

        playerTurnState.AddTransition(BattleStateInputs.Finish, finishState);
        playerTurnState.AddTransition(BattleStateInputs.AITurn, aiTurnState);

        stateMachine = new StateMachine<BattleStateInputs>(initializeState);
        isPlayerTurn = true;
    }

    // Update is called once per frame
    private void Update()
    {
        stateMachine.Update();

        if(isPlayerTurn)
        {
            if(usedShips.Count == playerShipContainer.childCount)
            {
                stateMachine.Transition(BattleStateInputs.AITurn);
                usedShips.Clear();
                isPlayerTurn = false;
            }
        }
        else
        {
            if(usedShips.Count == aiShipContainer.childCount)
            {
                stateMachine.Transition(BattleStateInputs.PlayerTurn);
                usedShips.Clear();
                isPlayerTurn = true;
                selectedShip = null;
            }
        }
    }

    public void PlayerShipDone(Ship ship)
    {
        usedShips.Add(ship);
        playerShips.Remove(ship);
    }

    public void AIShipDone(AIShip ship)
    {
        usedShips.Add(ship);
        aiShips.Remove(ship);
    }

    public static bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
