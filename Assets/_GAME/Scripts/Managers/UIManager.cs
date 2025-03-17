using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject gameLosePanel;
    [SerializeField] private GameObject menuBar;
    [Header("Settings")]
    [SerializeField] private GameObject throwStartingButton;
    [SerializeField] private GameObject upgradeHookPanel;


    private void Awake()
    {
        Hook.onThrowStarting += StartingThrow;
        Hook.onThrowEnding += EndingThrow;
    }
    private void OnDestroy()
    {
        Hook.onThrowStarting -= StartingThrow;
        Hook.onThrowEnding -= EndingThrow;
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
        upgradeHookPanel.SetActive(false);
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
        WaveManager.instance.StartWaves(0);
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
