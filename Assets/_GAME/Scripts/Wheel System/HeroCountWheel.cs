using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCountWheel : PointerMover
{
    private void Start()
    {
        if (stopButton != null)
            stopButton.onClick.AddListener(StopPointer);

        WheelSOConfig();
    }
    protected override void Apply(string slotName)
    {
        WheelManager.instance.heroes.Add(slotName);
        WheelManager.instance.wheelClickNumber += 1;

        WheelManager.instance.wheelStopButtons[0].interactable = false;
        WheelManager.instance.wheelStopButtons[1].interactable = false;
        WheelManager.instance.wheelStopButtons[2].interactable = true;


        WheelManager.instance.pointerMover[2].IsMoving(true);
    }
}
