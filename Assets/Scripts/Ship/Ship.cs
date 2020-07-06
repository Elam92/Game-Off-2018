using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ShipMovement))]
[RequireComponent(typeof(ShipShooting))]
[RequireComponent(typeof(ShipHealth))]
public class Ship : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool active = false;
    public bool turnFinished = false;

    protected string shipOwner;

    protected StateMachine<ShipStateInputs> stateMachine;

    protected ShipMovement shipMovement;
    protected ShipShooting shipWeapon;
    protected ShipHealth shipHealth;

    [SerializeField]
    protected Node currentNode;

    private Sprite portrait;


    protected void Awake()
    {
        shipMovement = GetComponent<ShipMovement>();
        shipWeapon = GetComponent<ShipShooting>();
        shipHealth = GetComponent<ShipHealth>();
        portrait = GetComponentInChildren<SpriteRenderer>().sprite;

        shipHealth.OnDeath += Death;

        // Assign the grid and ship to each other.
        if(currentNode == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Grid"));
            if(hit.collider != null)
            {
                Node node = hit.transform.GetComponent<Node>();

                if(node != null)
                {
                    currentNode = node;
                    node.unit = transform;
                    node.traversable = false;
                }
            }
        }
    }

    // Use this for initialization
    protected virtual void Start()
    {
        var shipIdleState = new ShipIdleState(this);
        var shipAttackState = new ShipAttackState(this, shipIdleState);
        var shipMoveState = new ShipMoveState(this, shipAttackState);
        var shipSelectedState = new ShipSelectedState(this, shipMoveState);
        var shipIsHitState = new ShipIsHitState(this, shipIdleState);

        shipIdleState.AddTransition(ShipStateInputs.Selected, shipSelectedState);
        shipIdleState.AddTransition(ShipStateInputs.IsHit, shipIsHitState);

        shipSelectedState.AddTransition(ShipStateInputs.Attack, shipAttackState);
        shipSelectedState.AddTransition(ShipStateInputs.Move, shipMoveState);
        shipSelectedState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        shipAttackState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        shipMoveState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        shipIsHitState.AddTransition(ShipStateInputs.Idle, shipIdleState);

        stateMachine = new StateMachine<ShipStateInputs>(shipIdleState);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.Update();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIController.Instance.ShowShipStats(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIController.Instance.HideShipStats();
    }


    private void Death(object sender, EventArgs e)
    {
        currentNode.traversable = true;
        currentNode.unit = null;
        shipHealth.OnDeath -= Death;
        Destroy(gameObject);
    }

    public void SetOwner(string owner)
    {
        shipOwner = owner;
    }

    public string GetOwner()
    {
        return shipOwner;
    }

    public int GetHealth()
    {
        return shipHealth.GetCurrentHealth();
    }

    public Sprite GetPortrait()
    {
        return portrait;
    }

    public void TakeDamage(int damage)
    {
        shipHealth.TakeDamage(damage);
        stateMachine.Transition(ShipStateInputs.IsHit);
    }

    public void Fire(Node targetNode)
    {
        shipWeapon.Fire(targetNode);
    }

    public void FinishTurn()
    {
        turnFinished = true;
        stateMachine.Transition(ShipStateInputs.Idle);
    }

    public bool IsFiring()
    {
        return shipWeapon.IsFiring();
    }

    public (Node[] range, Node[] targets)? ShowWeaponRange()
    {
        return shipWeapon.ShowWeaponRange(GetCurrentNode().gridPosition);
    }

    public int GetWeaponRange()
    {
        return shipWeapon.GetWeaponRange();
    }

    public int GetWeaponDamage()
    {
        return shipWeapon.GetWeaponDamage();
    }

    public void Move(Node targetNode)
    {
        shipMovement.MoveShip(currentNode, targetNode);
        currentNode = targetNode;
    }

    public bool IsMoving()
    {
        return shipMovement.IsMoving();
    }

    public Node[] ShowMovementRange()
    {
        return shipMovement.ShowMoveRange(currentNode);
    }

    public int GetMovementSpeed()
    {
        return shipMovement.GetMovementSpeed();
    }

    public Node GetCurrentNode()
    {
        return currentNode;
    }
}
