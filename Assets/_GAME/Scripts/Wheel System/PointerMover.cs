using UnityEngine;
using UnityEngine.UI;

public abstract class PointerMover : MonoBehaviour
{
    public WheelSO wheelSO;
    public WheelSO[] wheelSOs;
    public RectTransform[] heroSlots;     
    public Image[] heroSlotsImage;     
    public RectTransform pointer;         
    public float cycleDuration = 2f;      
    public Button stopButton;
    private int previouslyHighlightedIndex = -1;
    public float scaleMultiplier = 1.1f;
    public float scaleSpeed = 10f;

    private float elapsedTime = 0f;
    private bool isMoving = true;

    

    void Update()
    {
        if (!isMoving || heroSlots.Length < 2 || pointer == null)
            return;

        Vector2 left = heroSlots[0].anchoredPosition;
        Vector2 right = heroSlots[heroSlots.Length - 1].anchoredPosition;

        elapsedTime += Time.deltaTime;
        float t = (elapsedTime % cycleDuration) / (cycleDuration / 2f);
        t = Mathf.PingPong(t, 1f);
        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        pointer.anchoredPosition = Vector2.Lerp(left, right, smoothT);

        float minDistance = float.MaxValue;
        int closestSlotIndex = -1;
        Vector2 pointerPos = pointer.anchoredPosition;

        for (int i = 0; i < heroSlots.Length; i++)
        {
            float distance = Vector2.Distance(pointerPos, heroSlots[i].anchoredPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlotIndex = i;
            }
        }

        for (int i = 0; i < heroSlots.Length; i++)
        {
            Vector3 targetScale = (i == closestSlotIndex) ? Vector3.one * scaleMultiplier : Vector3.one;
            heroSlots[i].localScale = Vector3.Lerp(heroSlots[i].localScale, targetScale, Time.deltaTime * scaleSpeed);
        }

        previouslyHighlightedIndex = closestSlotIndex;
    }

    public void WheelSOConfig()
    {
        int randomNumber = Random.Range(0, wheelSOs.Length);
        wheelSO = wheelSOs[randomNumber];


        for (int i = 0; i < heroSlots.Length; i++)
        {
            heroSlotsImage[i].GetComponent<Image>().sprite = wheelSO.slotsImage[i];
        }
    }

    public void StopPointer()
    {
        isMoving = false;

        float minDistance = float.MaxValue;
        int closestSlotIndex = -1;

        Vector2 pointerPos = pointer.anchoredPosition;

        for (int i = 0; i < heroSlots.Length; i++)
        {
            float distance = Vector2.Distance(pointerPos, heroSlots[i].anchoredPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestSlotIndex = i;
            }
        }

        Debug.Log("Ok durdu! Hiza: Slot " + (closestSlotIndex));
        string name = wheelSO.slotsName[closestSlotIndex];
        Debug.Log("THIS IS"+ name);
        Apply(name);
    }

    protected abstract void Apply(string slotName);

    public bool IsMoving(bool state)
    {
        return isMoving = state;
    }
}
