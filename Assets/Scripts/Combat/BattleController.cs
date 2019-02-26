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
    private Ship selectedShip;

    public static BattleController Instance { get; private set; }

    public Ship SelectedShip
    {
        get
        {
            return selectedShip;
        }
        set
        {
            if (selectedShip != null)
            {
                selectedShip.active = false;
                if(selectedShip.turnFinished)
                {
                    Debug.Log(selectedShip + " IS DONE");
                    ShipDone(selectedShip);
                }
            }
            selectedShip = value;
        }
    }

    private static bool isPlayerTurn;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Use this for initialization
    private void Start()
    {
        usedShips = new List<Ship>();
        selectedShip = null;

        // Add Ships in Scene
        playerShips = new List<Ship>();
        foreach (Transform ship in playerShipContainer)
        {
            playerShips.Add(ship.GetComponent<Ship>());
        }

        aiShips = new List<AIShip>();
        foreach (Transform aiShip in aiShipContainer)
        {
            aiShips.Add(aiShip.GetComponent<AIShip>());
        }

        AISimple ai = new AISimple(aiShips, playerShips);

        var finishState = new FinishBattleState(this);
        var aiTurnState = new AITurnState(this, ai, finishState);
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

        if (isPlayerTurn)
        {
            if (usedShips.Count == playerShipContainer.childCount)
            {
                stateMachine.Transition(BattleStateInputs.AITurn);
                usedShips.Clear();
                isPlayerTurn = false;
            }
        }
        else
        {
            if (usedShips.Count == aiShipContainer.childCount)
            {
                stateMachine.Transition(BattleStateInputs.PlayerTurn);
                usedShips.Clear();
                isPlayerTurn = true;
            }
        }
    }

    private void ShipDone(Ship ship)
    {
        usedShips.Add(ship);
        Debug.Log("USED SHIPS: " + usedShips.Count);
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
