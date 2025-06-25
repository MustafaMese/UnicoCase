using UnityEngine;

namespace CustomEvent
{
    public class SelectionCompleted : ICustomEvent
    {
        public Vector2Int Tile;
        public DefenceItemType Type;
    }
}