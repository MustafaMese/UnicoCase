using System.Collections.Generic;
using DefenceItems;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Data/DefenceItemData")]
public class DefenceItemData : ScriptableObject
{
    public List<Data> defenceItems;
    public Projectile projectilePrefab;
    
    [System.Serializable]
    public class Data
    {
        public int health;
        public int damage;
        public float range;
        public float interval;
        public DefenceItemType type;
        public DirectionType directionType;
        public DefenceItem defenceItemPrefab;
    }
}