using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseManager : MonoBehaviour
{
    public static EnemyBaseManager instance;
    [SerializeField] private UIManager uiManager;
    public Vector3 spawnOriginPosition = Vector3.zero;
    public Transform spawnRoot;
    public float cellSize = 1.5f;
    public int aliveCount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public void SpawnBase(GridEnemyBaseData baseData)
    {
        int gridSize = baseData.gridSize;

        foreach (var slot in baseData.entitiesInGrid)
        {
            if (slot.entityType == BaseEntityType.Empty || slot.entityData == null)
                continue;

            Vector3 spawnPos = GetWorldPosition(slot.x, slot.y, gridSize);
            GameObject instance = Instantiate(slot.entityData.prefab, spawnPos, Quaternion.identity, spawnRoot);
            instance.name = $"{slot.entityType}_({slot.x},{slot.y})";
        }
    }

    private Vector3 GetWorldPosition(int x, int y, int gridSize)
    {
        float offset = (gridSize - 1) * 0.5f * cellSize;
        return new Vector3(x * cellSize - offset, y * cellSize - offset, 0f) + spawnOriginPosition;
    }

    public void RegisterObject()
    {
        aliveCount++;
    }

    public void UnRegisterObject()
    {
        aliveCount--;

        if (aliveCount <= 0)
        {
            uiManager.GameWinPanel();
        }
    }
}
