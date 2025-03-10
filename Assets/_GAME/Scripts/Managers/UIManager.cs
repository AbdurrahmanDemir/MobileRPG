using UnityEngine;

public class UIManager : MonoBehaviour
{
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


    void StartingThrow()
    {
        throwStartingButton.SetActive(false);
        upgradeHookPanel.SetActive(false);
    }

    void EndingThrow()
    {
        upgradeHookPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartWave()
    {
        upgradeHookPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
