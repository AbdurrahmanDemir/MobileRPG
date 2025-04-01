using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject[] allHeroes;
    [SerializeField] private Transform[] creatHeroPosition;
    [SerializeField] private Hook hook;

    private void Awake()
    {
        Hook.onThrowEnding += CreatHeroes;
    }
    private void OnDestroy()
    {
        Hook.onThrowEnding -= CreatHeroes;
    }

    public void CreatHeroes()
    {
        for (int i = 0; i < hook.hookedHero.Count; i++)
        {            
            switch (hook.hookedHero[i].GetHeroName())
            {
                case "Angel":
                    int RandomPos = Random.Range(0, creatHeroPosition.Length);
                    Instantiate(allHeroes[0], creatHeroPosition[RandomPos]);
                    break;
                case "Range Angel":
                    int RandomPos1 = Random.Range(0, creatHeroPosition.Length);
                    Instantiate(allHeroes[1], creatHeroPosition[RandomPos1]);
                    break;
                case "Angel Man":
                    int RandomPos2 = Random.Range(0, creatHeroPosition.Length);
                    Instantiate(allHeroes[2], creatHeroPosition[RandomPos2]);
                    break;
            }
        }
    }
    public void GameSpeedController()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 3;
        else
            Time.timeScale = 1;
    }
}
