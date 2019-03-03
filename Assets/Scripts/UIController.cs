using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
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
        if (ship != null)
        {
            healthUI.text = ship.GetHealth().ToString();
            movementUI.text = ship.GetMovementSpeed().ToString();
            rangeUI.text = ship.GetWeaponRange().ToString();
            damageUI.text = ship.GetWeaponDamage().ToString();
        }
        else
        {
            healthUI.text = "";
            movementUI.text = "";
            rangeUI.text = "";
            damageUI.text = "";
        }
    }
}
