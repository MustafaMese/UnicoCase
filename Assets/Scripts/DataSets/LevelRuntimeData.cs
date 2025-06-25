using System.Collections.Generic;
using System.Threading.Tasks;
using Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/LevelRuntimeData")]
public class LevelRuntimeData : ScriptableObject, ILevelRuntimeData
{
    [SerializeField] private LevelData levelData;
    
    private GameServices _services;
    private int _levelIndex;

    public Task Initialize(GameServices services, int levelIndex)
    {
        _levelIndex = levelIndex;
        
        _services = services;
        _services.RegisterData<ILevelRuntimeData, LevelRuntimeData>(this);
        
        return Task.CompletedTask;
    }

    public Vector2Int BoardSize() => levelData.boardSize;
    public Block BlockPrefab() => levelData.blockPrefab;
    public float EnemySpawnInterval() => levelData.enemySpawnInterval;
    public List<EnemyType> Enemies() => levelData.items[_levelIndex].enemies;
    public List<DefenceItemType> DefenceItems() => levelData.items[_levelIndex].defenceItems;


    public bool HasEnemies() => Enemies().Count > 0;
}

public interface ILevelRuntimeData
{
    Task Initialize(GameServices services, int levelIndex);
}