using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectHeroWheel : PointerMover
{
    [SerializeField] private GameObject[] bonusAlert;

    private void Start()
    {
        if (stopButton != null)
            stopButton.onClick.AddListener(StopPointer);

        WheelSOConfig();

        BonusAlertImage();
    }
    protected override void Apply(string slotName)
    {
        WheelManager.instance.heroes.Add(slotName);
        WheelManager.instance.wheelStopButtons[0].interactable = false;
        WheelManager.instance.wheelStopButtons[1].interactable = true;
        WheelManager.instance.wheelStopButtons[2].interactable = false;

        WheelManager.instance.pointerMover[1].IsMoving(true);
        WheelManager.instance.pointerMover[2].IsMoving(false);


    }

    public void BonusAlertImage()
    {
        for (int i = 0; i < heroSlots.Length; i++)
        {
            bonusAlert[i].SetActive(false);

            switch (wheelSO.slotsName[i])
            {
                case "RangeAngel + Ice":
                    bonusAlert[i].SetActive(true);
                    break;
                case "Range + Man":
                    bonusAlert[i].SetActive(true);
                    break;
            }
        }
    }

}
