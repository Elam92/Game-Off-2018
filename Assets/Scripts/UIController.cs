using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField]
    private GameObject shipStatsContainer;
    [SerializeField]
    private TextMeshProUGUI shipOwner;
    [SerializeField]
    private Image unitPortrait;
    [SerializeField]
    private TextMeshProUGUI healthUI;
    [SerializeField]
    private TextMeshProUGUI movementUI;
    [SerializeField]
    private TextMeshProUGUI rangeUI;
    [SerializeField]
    private TextMeshProUGUI damageUI;
    [SerializeField]
    private Button endTurnButton;
    [SerializeField]
    private Button endGameButton;
#pragma warning restore 649

    private Color noPortrait = new Color(1,1,1,0);
    private Color hasPortrait = Color.white;
    private Ship selectedShip;

    public static UIController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        healthUI.text = "";
        movementUI.text = "";
        rangeUI.text = "";
        damageUI.text = "";

        endTurnButton.onClick.AddListener(EndTurn);
        endGameButton.onClick.AddListener(EndGame);
    }

    private void EndTurn()
    {
        BattleController.Instance.EndTurn();
    }

    private void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowShipStats(Ship ship)
    {
        shipStatsContainer.SetActive(true);

        if (ship != null)
        {
            shipOwner.text = ship.GetOwner() + " Ship";

            Sprite shipPortrait = ship.GetPortrait();
            if(shipPortrait != null)
            {
                unitPortrait.sprite = shipPortrait;
                unitPortrait.color = hasPortrait;
            }
            else
            {
                unitPortrait.sprite = null;
                unitPortrait.color = noPortrait;
            }

            healthUI.text = ship.GetHealth().ToString();
            movementUI.text = ship.GetMovementSpeed().ToString();
            rangeUI.text = ship.GetWeaponRange().ToString();
            damageUI.text = ship.GetWeaponDamage().ToString();
        }
        else
        {
            shipOwner.text = "";
            unitPortrait.sprite = null;
            healthUI.text = "";
            movementUI.text = "";
            rangeUI.text = "";
            damageUI.text = "";
        }
    }

    public void HideShipStats()
    {
        if(selectedShip == null)
        {
            shipStatsContainer.SetActive(false);
        } else
        {
            ShowShipStats(selectedShip);
        }
    }

    public void SetSelectedShip(Ship ship)
    {
        selectedShip = ship;
    }
}
