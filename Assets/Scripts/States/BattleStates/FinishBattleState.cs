using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishBattleState : State<BattleStateInputs>
{

    private BattleController controller;

    public FinishBattleState(BattleController controller)
    {
        this.controller = controller;
    }

    public override State<BattleStateInputs> Update()
    {

        if (controller.aiShipContainer.childCount <= 0)
        {
            int index = SceneManager.GetActiveScene().buildIndex + 1;

            if (index >= SceneManager.sceneCountInBuildSettings)
            {
                index = 0;
            }
            SceneManager.LoadSceneAsync(index);
        }
        else if (controller.playerShipContainer.childCount <= 0)
        {
            SceneManager.LoadScene("MainMenu");
        }

        return null;
    }
}
