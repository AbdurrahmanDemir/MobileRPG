using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FactoryHouse : Building
{
    [Header("Barbar Settings")]
    public Worker worker;
    public WorkerController workerController;

    int upgradeLevel=1;
    public TextMeshProUGUI upgradePrice;
    public TextMeshProUGUI currentTimeText;

    private void Start()
    {
        base.Start();

        if (workerController.TryPurchaseWorker(0))
            StartProduction();

        upgradePrice.text= (50*upgradeLevel).ToString();
        currentTimeText.text = $"1 worker production: <color=#00FF00>{productionTime}</color> seconds";
    }

    protected override void OnProductionComplete()
    {
        worker.AddIdleWorker(1);
        
        if (productionSlider != null)
            productionSlider.value = 0;

        if (workerController.TryPurchaseWorker(0))
            StartProduction();
    }

    public void UpgradeHouse()
    {
        if (DataManager.instance.TryPurchaseGold(50 * upgradeLevel))
        {
            productionTime -= 0.1f;
            upgradeLevel++;
            upgradePrice.text = (50 * upgradeLevel).ToString();
            currentTimeText.text = $"1 worker production: <color=#00FF00>{productionTime}</color> seconds";

        }
    }
}
