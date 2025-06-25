using System.Collections.Generic;
using Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public List<Data> enemies;
    
    [System.Serializable]
    public class Data
    {
        public Enemy enemyPrefab;
        public EnemyType enemyType;
        public int health;
        public float speed;
        public int damage;
        public float jumpPower;
        public int jumpCount;
    }
}