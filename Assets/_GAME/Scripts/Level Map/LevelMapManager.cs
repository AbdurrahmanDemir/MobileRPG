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
        levelEpisodeIndex = PlayerPrefs.GetInt("LevelEpisodeIndex", 0);
        LevelMapButtonUpdate();

        // Her episode için ayrý level kayýtlarý
        int currentLevel = GetCurrentLevelForEpisode(levelEpisodeIndex);
        Debug.Log($"Episode {levelEpisodeIndex} - Current Level: {currentLevel}");
    }

    public void LevelMapButtonUpdate()
    {
        // Önce tüm episode map'lerini kapat
        for (int episodeIndex = 0; episodeIndex < levelEpisodes.Length; episodeIndex++)
        {
            levelEpisodes[episodeIndex].episodeLevelMap.SetActive(false);
        }

        // Sadece aktif episode'u aç
        levelEpisodes[levelEpisodeIndex].episodeLevelMap.SetActive(true);

        // Bu episode için current level'i al
        int currentLevel = GetCurrentLevelForEpisode(levelEpisodeIndex);

        for (int i = 0; i < levelEpisodes[levelEpisodeIndex].episodeDetails.Length; i++)
        {
            bool isActive = (i < currentLevel);
            levelEpisodes[levelEpisodeIndex].levelButton[i].SetActive(true);

            levelEpisodes[levelEpisodeIndex].levelButtonText[i].text = (i + 1).ToString();

            Image buttonImage = levelEpisodes[levelEpisodeIndex].levelButton[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                if (isActive)
                {
                    // Aktif level butonlarý için görsel ayarlarý
                    buttonImage.color = Color.white; // veya istediðiniz renk
                }
                else
                {
                    // Pasif level butonlarý için görsel ayarlarý  
                    buttonImage.color = Color.gray; // veya istediðiniz renk
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

    // Her episode için ayrý level kayýtlarý tutan fonksiyon
    private int GetCurrentLevelForEpisode(int episodeIndex)
    {
        string key = $"Episode_{episodeIndex}_CurrentLevel";
        return PlayerPrefs.GetInt(key, 1); // Her episode 1. level'den baþlar
    }

    // Episode için level kayýdýný güncelleyen fonksiyon
    public void SetCurrentLevelForEpisode(int episodeIndex, int level)
    {
        string key = $"Episode_{episodeIndex}_CurrentLevel";
        PlayerPrefs.SetInt(key, level);
        PlayerPrefs.Save();
    }

    // Episode'un açýk olup olmadýðýný kontrol eden fonksiyon
    public bool IsEpisodeUnlocked(int episodeIndex)
    {
        if (episodeIndex == 0) return true; // Ýlk episode her zaman açýk

        // Önceki episode'un son levelini tamamlamýþ mý kontrol et
        int previousEpisodeMaxLevel = GetCurrentLevelForEpisode(episodeIndex - 1);
        int previousEpisodeTotalLevels = levelEpisodes[episodeIndex - 1].episodeDetails.Length;

        return previousEpisodeMaxLevel > previousEpisodeTotalLevels;
    }

    // Episode seçme fonksiyonu (UI'dan çaðrýlabilir)
    public void SelectEpisode(int episodeIndex)
    {
        if (IsEpisodeUnlocked(episodeIndex))
        {
            levelEpisodeIndex = episodeIndex;
            PlayerPrefs.SetInt("LevelEpisodeIndex", levelEpisodeIndex);
            PlayerPrefs.Save();
            LevelMapButtonUpdate();
        }
        else
        {
            Debug.Log($"Episode {episodeIndex} henüz açýlmamýþ!");
            // Kullanýcýya bilgi mesajý gösterebilirsiniz
        }
    }

    public void LevelDetailsPanel(int index)
    {
        OpenPanel(levelDetailsPanel);
        levelName.text = "LEVEL: " + levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.baseName;
        levelDate.text = levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.episodeDate;
        levelType.text = "Level Type: " + levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.levelType.ToString();
        levelDescription.text = levelEpisodes[levelEpisodeIndex].episodeDetails[index].episodeData.cardDescription;
        levelPlayButton.onClick.RemoveAllListeners();
        levelPlayButton.onClick.AddListener(() =>
        {
            // Oynanan level bilgisini kaydet
            PlayerPrefs.SetInt("PlayingEpisode", levelEpisodeIndex);
            PlayerPrefs.SetInt("PlayingLevel", index);
            PlayerPrefs.Save();

            // Gerçek asset index'ini hesapla (episode baþlangýç index'i + level index)
            int realAssetIndex = CalculateAssetIndex(levelEpisodeIndex, index);
            enemyBaseManager.LoadLevel(realAssetIndex);
            uiManager.GameUIStageChanged(UIGameStage.Game);
            levelEpisodes[levelEpisodeIndex].episodeLevelMap.SetActive(false);
        });
    }

    // Episode ve level index'ine göre gerçek asset index'ini hesaplar
    private int CalculateAssetIndex(int episodeIndex, int levelIndex)
    {
        int totalIndex = 0;

        // Önceki episode'larýn level sayýlarýný topla
        for (int i = 0; i < episodeIndex; i++)
        {
            totalIndex += levelEpisodes[i].episodeDetails.Length;
        }

        // Mevcut episode'daki level index'ini ekle
        totalIndex += levelIndex;

        Debug.Log($"Episode {episodeIndex}, Level {levelIndex} -> Asset Index: {totalIndex}");
        return totalIndex;
    }

    // Episode'daki toplam level sayýsýný döndürür (EnemyBaseManager için)
    public int GetEpisodeLevelCount(int episodeIndex)
    {
        if (episodeIndex >= 0 && episodeIndex < levelEpisodes.Length)
        {
            return levelEpisodes[episodeIndex].episodeDetails.Length;
        }
        return 0;
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