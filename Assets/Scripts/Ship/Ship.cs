using System;
using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
[RequireComponent(typeof(ShipShooting))]
[RequireComponent(typeof(ShipHealth))]
public class Ship : MonoBehaviour
{
    public bool active = false;
    public bool turnFinished = false;

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

    private void Death(object sender, EventArgs e)
    {
        currentNode.traversable = true;
        currentNode.unit = null;
        Destroy(gameObject);
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

    public bool IsFiring()
    {
        return shipWeapon.IsFiring();
    }

    public Node[] ShowWeaponRange()
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
