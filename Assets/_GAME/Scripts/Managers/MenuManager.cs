using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Arena Settings")]
    int arenaIndex;
    int completedArenaIndex;
    [SerializeField] private TextMeshProUGUI arenaName;
    [SerializeField] private Image arenaImage;
    [SerializeField] private TextMeshProUGUI arenaDescription;
    [Header("Arena Elements")]
    [SerializeField] private string[] arenaNames;
    [SerializeField] private Sprite[] arenaImages;
    [SerializeField] private string[] arenaDescriptions;

    private void Start()
    {
        arenaIndex = /*PlayerPrefs.GetInt("WaveIndex", 0);*/ 2;
        completedArenaIndex = arenaIndex;
        ArenaPanelUpdate(arenaIndex);
    }
    public void ArenaPanelUpdate(int index)
    {
        arenaName.text = arenaNames[index];
        arenaImage.sprite = arenaImages[index];

        if (index == completedArenaIndex)
        {
            arenaDescription.text = arenaDescriptions[index];
        }
        else if(index > completedArenaIndex)
        {
            arenaDescription.text = "Bu arenaya daha gelmedin";
        }
        else if (index < completedArenaIndex)
        {
            arenaDescription.text = "Bu arenayý tamamladýn";
        }
    }
    public void ArenaChangeLeft()
    {
        if(arenaIndex == 0)
            return;
        arenaIndex--;
        ArenaPanelUpdate(arenaIndex);
    }
    public void ArenaChangeRight()
    {
        if(arenaIndex==arenaNames.Length)
            return;
        arenaIndex++;
        ArenaPanelUpdate(arenaIndex);
    }
}
