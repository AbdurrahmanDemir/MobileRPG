using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;

public class WorkerController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private int workerCount;
    [SerializeField] private TextMeshProUGUI workerCountText;

    private void Start()
    {
        AddWorker(5);
    }
    public void AddWorker(int value)
    {
        workerCount += value;
        UpdateWorkerText();

    }
    public bool TryPurchaseWorker(int price)
    {
        if (price <= workerCount)
        {
            workerCount -= price;
            UpdateWorkerText();
            return true;
        }
        else
        {
            PopUpController.instance.OpenPopUp("NOT ENOUGH WORKER");
        }
        return false;
    }

    private void UpdateWorkerText()
    {
        workerCountText.text = workerCount.ToString();
    }

}
