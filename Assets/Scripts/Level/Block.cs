using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector2Int _tile;

    public void Initialize(Vector2Int tile)
    {
        _tile = tile;
        gameObject.name = $"{tile.x},{tile.y}";
    }
        
    public Vector3 GetSpawnPosition()
    {
        return transform.position + Vector3.up * transform.localScale.y / 2 + Vector3.back * transform.localScale.z / 2;
    }
}