using System;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/LevelReadonlyData")]
public class LevelData : ScriptableObject
{
    public List<Data> items;
    public Vector2Int boardSize;
    public Block blockPrefab;
    public float enemySpawnInterval = 1f;
    
    [Serializable]
    public class Data
    {
        public List<DefenceItemType> defenceItems;
        public List<EnemyType> enemies;
    }
}

