using DG.Tweening;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;   

    [Header("Elements")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLosePanel;
    [SerializeField] private GameObject menuBar;
    [Header("Settings")]
    [SerializeField] private GameObject throwStartingButton;
    [SerializeField] private GameObject upgradeHookPanel;
    [Header("Level")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private TowerController towerController;


    private void Awake()
    {
        Hook.onThrowStarting += StartingThrow;
        Hook.onThrowEnding += EndingThrow;

        TowerController.onGameLose += GameLosePanel;
        WaveManager.onGameWin += GameWinPanel;
    }
    private void OnDestroy()
    {
        Hook.onThrowStarting -= StartingThrow;
        Hook.onThrowEnding -= EndingThrow;

        TowerController.onGameLose -= GameLosePanel;
        WaveManager.onGameWin -= GameWinPanel;


    }

    private void Start()
    {
        GameUIStageChanged(UIGameStage.Menu);
    }
    void StartingThrow()
    {
        throwStartingButton.SetActive(false);
        upgradeHookPanel.SetActive(false);
    }

    void EndingThrow()
    {
        upgradeHookPanel.SetActive(true);
        throwStartingButton.SetActive(true);
    }

    public void StartWave()
    {
        upgradeHookPanel.SetActive(false);
        throwStartingButton.SetActive(true);
    }
    public void PlayButton()
    {
        GameUIStageChanged(UIGameStage.Game);
        int waveIndex = PlayerPrefs.GetInt("WaveIndex", 0);
        WaveManager.instance.StartWaves(waveIndex);
    }
    public void GameLosePanel()
    {
        GameUIStageChanged(UIGameStage.GameLose);
    }
    public void GameLoseButton()
    {
        GameUIStageChanged(UIGameStage.Menu);

        for (int i = 0; i < enemyParent.childCount; i++)
        {
            Destroy(enemyParent.GetChild(i).gameObject);
        }

        towerController.ResetTower();
        gameManager.PowerUpReset();
    }

    public void GameWinPanel()
    {
        GameUIStageChanged(UIGameStage.GameWin);

        for (int i = 0; i < enemyParent.childCount; i++)
        {
            Destroy(enemyParent.GetChild(i).gameObject);
        }

        towerController.ResetTower();
        gameManager.PowerUpReset();
    }
    public void GameWinButton()
    {
        GameUIStageChanged(UIGameStage.Menu);

        for (int i = 0; i < enemyParent.childCount; i++)
        {
            Destroy(enemyParent.GetChild(i).gameObject);
        }

        towerController.ResetTower();
    }
    public void GameUIStageChanged(UIGameStage stage)
    {
        switch (stage)
        {
            case UIGameStage.Menu:
                menuPanel.SetActive(true);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(false);
                menuBar.SetActive(true);
                break;
            case UIGameStage.Game:
                menuPanel.SetActive(false);
                gamePanel.SetActive(true);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(false);
                menuBar.SetActive(false);

                break;
            case UIGameStage.GameWin:
                menuPanel.SetActive(false);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(true);
                gameLosePanel.SetActive(false);


                break;
            case UIGameStage.GameLose:
                menuPanel.SetActive(false);
                gamePanel.SetActive(false);
                gameWinPanel.SetActive(false);
                gameLosePanel.SetActive(true);
                break;

            default:
                break;
        }

    }

    

}
public enum UIGameStage
{
    Menu,
    Game,
    GameWin,
    GameLose
}
