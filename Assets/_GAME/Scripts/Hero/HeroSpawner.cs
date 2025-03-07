using UnityEngine;

public class HeroSpawner : MonoBehaviour
{
    void Awake()
    {
        for (int i = 0; i < heroTypes.Length; i++)
        {
            int num = 0;
            while (num < heroTypes[i].heroCount)
            {
                HookedHero hero = UnityEngine.Object.Instantiate<HookedHero>(heroPrefabs);
                hero.Type = heroTypes[i];
                hero.ResetHero();
                num++;
            }
        }
    }

    [SerializeField]
    private HookedHero heroPrefabs;

    [SerializeField]
    private HookedHero.HeroType[] heroTypes;
}
