using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class LevelMapManager : MonoBehaviour
{

    [SerializeField] private EnemyBaseManager enemyBaseManager;
    [SerializeField] private UIManager uiManager;
   [Header("Elements")]
    [SerializeField] private int levelEpisodeIndex;
    [Header("Settings")]
    [SerializeField] private LevelEpisode[] levelEpisodes;

    [Header("Level Details Panel")]
    [SerializeField] private GameObject levelDetailsPanel;
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private TextMeshProUGUI levelDate;
    [SerializeField] private TextMeshProUGUI levelType;
    [SerializeField] private TextMeshProUGUI levelDescription;
    [SerializeField] private Button levelPlayButton;

    private void Start()
    {
        levelEpisodeIndex = PlayerPrefs.GetInt("LevelEpisodeIndex",0);
        LevelMapButtonUpdate();
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        Debug.Log("LEVEL"+ currentLevel);
    }

    public void LevelMapButtonUpdate()
    {
        levelEpisodes[levelEpisodeIndex].episodeLevelMap.SetActive(true);

        int currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1); 

        for (int i = 0; i < levelEpisodes[levelEpisodeIndex].episodeDetails.Length; i++)
        {
            bool isActive = (i < currentLevel);
            levelEpisodes[levelEpisodeIndex].levelButton[i].SetActive(true); 

            // Text güncelle
            levelEpisodes[levelEpisodeIndex].levelButtonText[i].text = (i + 1).ToString();

            // Butonun Image component'ini al
            Image buttonImage = levelEpisodes[levelEpisodeIndex].levelButton[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                if (isActive)
                {
                }
                else
                {

                }
            }

            Button button = levelEpisodes[levelEpisodeIndex].levelButton[i].GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            if (isActive)
            {
                int capturedIndex = i;
                button.onClick.AddListener(() => LevelDetailsPanel(capturedIndex));
                button.interactable = true; 
            }
            else
            {
                button.interactable = false; 
            }
        }
    }


    public void LevelDetailsPanel(int index)
    {
        OpenPanel(levelDetailsPanel);
        levelName.text = "LEVEL: "+ levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.baseName;
        levelDate.text = levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.episodeDate;
        levelType.text = "Level Type: " + levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.levelType.ToString();
        levelDescription.text = levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.cardDescription;
        levelPlayButton.onClick.RemoveAllListeners();
        levelPlayButton.onClick.AddListener(() =>
        {
            enemyBaseManager.LoadLevel(index);
            //enemyBaseManager.SpawnBase(levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData);
            uiManager.GameUIStageChanged(UIGameStage.Game);
            levelEpisodes[levelEpisodeIndex].episodeLevelMap.SetActive(false);
            //levelEpisodes[levelEpisodeIndex].episodeArena.SetActive(true);
        });

    }
    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
}

[System.Serializable]
public struct LevelEpisode
{
    [Header("Episode Info")]
    public string episodeName;
    public int episodeNumber;
    public EpisodeDetails[] episodeDetails;

    [Header("Episode Elements")]
    public GameObject episodeLevelMap;
    public GameObject episodeArena;
    public GameObject[] levelButton;
    public TextMeshProUGUI[] levelButtonText;

}
[System.Serializable]

public struct EpisodeDetails
{
    public GridEnemyBaseData episodeData;
}
