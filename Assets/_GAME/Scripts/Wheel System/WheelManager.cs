using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class WheelManager : MonoBehaviour
{
    [Header("Elements")]
    public static WheelManager instance;
    [SerializeField] private GameManager gameManager;
    public PointerMover[] pointerMover;
    [SerializeField] private SelectBonusWheel selectBonusWheel;
    [SerializeField] private SelectHeroWheel selectHeroWheel;
    [HideInInspector] public List<string> heroes;
    [HideInInspector] public int wheelClickNumber=0;
    public Button[] wheelStopButtons;
    public Button attackButton;

    [Header("Hero & Enemys")]
    [SerializeField] private Transform heroParent;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private bool isOver=false;
    [SerializeField] private bool isStart=false;

    [Header("Round System")]
    [SerializeField] private int roundNumber=0;
    public int RoundNumber=> roundNumber;
    [SerializeField] private bool roundState=false;
    public bool RoundState=> roundState;

    [Header("Round UI")]
    [SerializeField] private GameObject roundPanel;
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject wheelPanel;

    [Header("Stats Settings")]
    [SerializeField] private Slider heroTotalHealthSlider;
    [SerializeField] private Slider enemyTotalHealthSlider;
    [SerializeField] private TextMeshProUGUI heroTotalHealthText;
    [SerializeField] private TextMeshProUGUI enemyTotalHealthText;
    int totalHeroHealth;
    bool isHeroHealthCalculate;
    int totalEnemyHealth;

    [Header("Action")]
    public Action onGameLose;


    [Header("Camera")]
    [SerializeField] private CameraTransition cameraTransition;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Hero.OnAnyHeroHealthChanged += HeroTotalHealthCalculate;
        Enemy.OnAnyEnemyHealthChanged += EnemyTotalHealthCalculate;

        isOver = false;  
        isStart = false; 
    }

    private void OnDisable()
    {
        Hero.OnAnyHeroHealthChanged -= HeroTotalHealthCalculate;
        Enemy.OnAnyEnemyHealthChanged -= EnemyTotalHealthCalculate;
    }
    private void OnDestroy()
    {
        onGameLose = null;

    }

    private void Start()
    {
        heroes = new List<string>();
        wheelStopButtons[0].interactable = true;
        wheelStopButtons[1].interactable = false;
        wheelStopButtons[2].interactable = false;

        pointerMover[0].IsMoving(true);
        pointerMover[1].IsMoving(false);
        pointerMover[2].IsMoving(false);

        attackButton.interactable = false;


        cameraTransition.MoveToTarget();



    }
    private void Update()
    {
        if (heroParent.childCount <= 0 && isStart && !isOver)
        {
            isOver = true;
            onGameLose?.Invoke();
        }



    }
    public void NewRound()
    {
        Time.timeScale = 1;
        wheelStopButtons[0].interactable = true;
        wheelStopButtons[1].interactable = false;
        wheelStopButtons[2].interactable = false;

        pointerMover[0].IsMoving(true);
        pointerMover[1].IsMoving(false);
        pointerMover[2].IsMoving(false);

        attackButton.interactable = false;

        roundNumber++;
        roundState = true;
        for (int i = 0; i < pointerMover.Length; i++)
        {
            pointerMover[i].WheelSOConfig();
        }

        selectBonusWheel.BonusAlertImage();
        selectHeroWheel.BonusAlertImage();

        StartCoroutine(RoundPanel());
        TogglePanel(wheelPanel);
        TogglePanel(statsPanel);
        gameManager.HeroesStartingPosition();

    }
    public void StartNewRound()
    {
        roundState = false;
        TogglePanel(wheelPanel);
        TogglePanel(statsPanel);
        gameManager.CreatHeroes();
        
        heroes.Clear();
        isStart = true;



        cameraTransition.MoveToOriginal();
    }
    IEnumerator RoundPanel()
    {
        cameraTransition.MoveToTarget();
        TogglePanel(roundPanel);
        roundText.text = "NEW WAVE: " + roundNumber.ToString();
        yield return new WaitForSeconds(4f);
        TogglePanel(roundPanel);

    }

    public void HeroList()
    {
        Debug.Log(heroParent.childCount);
    }

    public void HeroTotalHealthCalculate()
    {
        totalHeroHealth = 0; 
        for (int i = 0; i < heroParent.childCount; i++)
        {
            Hero hero = heroParent.GetChild(i).GetComponent<Hero>();
            if (hero != null)
                totalHeroHealth += hero.health;
        }

        heroTotalHealthText.text = totalHeroHealth.ToString();
        heroTotalHealthSlider.maxValue = 2000;
        heroTotalHealthSlider.value = totalHeroHealth; 
    }

    public void EnemyTotalHealthCalculate()
    {
        totalEnemyHealth = 0;

        for (int i = 0; i < enemyParent.childCount; i++)
        {
            Enemy enemy = enemyParent.GetChild(i).GetComponent<Enemy>();
            if (enemy != null)
                totalEnemyHealth += enemy.health;
        }

        enemyTotalHealthText.text = totalEnemyHealth.ToString();
        enemyTotalHealthSlider.maxValue = 2000;
        enemyTotalHealthSlider.value = totalEnemyHealth;
    }


    public void TogglePanel(GameObject gameObject)
    {
        if (gameObject.activeSelf)
        {
            gameObject.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
            
        }
        else
        {
            gameObject.SetActive(true);
            gameObject.transform.localScale = Vector3.zero;
            gameObject.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
        }
    }
}
