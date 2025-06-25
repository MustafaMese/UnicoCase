using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BoardService : IBoardService
{
    private GameServices _services;
    private Dictionary<Vector2Int, Block> _blocks;

    public async Task Initialize(GameServices services)
    {
        _services = services;
        _blocks = new Dictionary<Vector2Int, Block>();
        _services.RegisterService<IBoardService, BoardService>(this);
        
        await CreateBoard();
    }

    private async Task CreateBoard()
    {
        var data = _services.ResolveData<ILevelRuntimeData, LevelRuntimeData>();
        
        var boardSize = data.BoardSize();
        var blockPrefab = data.BlockPrefab();
        
        var startPoint = new Vector3(
            -boardSize.x / 2f * blockPrefab.transform.localScale.x,
            0f,
            -boardSize.y / 2f * blockPrefab.transform.localScale.z
        );

        var tasks = new List<Task>();
            
        for (int y = 0; y < boardSize.y; y++)
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                var posX = startPoint.x + x * blockPrefab.transform.localScale.x;
                var posY = startPoint.z + y * blockPrefab.transform.localScale.z;
                var key = new Vector2Int((int)posX, (int)posY);
                
                var block = Object.Instantiate(
                    blockPrefab,
                    startPoint + 
                        new Vector3(
                            x * blockPrefab.transform.localScale.x, 
                            0f, 
                            y * blockPrefab.transform.localScale.z),
                    Quaternion.identity
                );
                
                _blocks.Add(key, block);
                block.Initialize(key);
                    
                var task = block.transform.DOScale(blockPrefab.transform.localScale, 0.5f).AsyncWaitForCompletion();
                tasks.Add(task);

                await Task.Delay(50);
            }
        }
            
        await Task.WhenAll(tasks);
    }

    public bool IsDefenceItemPositionValid(Vector2Int tile)
    {
        return tile.y >= 0 && _blocks.ContainsKey(tile);
    }

    public Vector3? GetRandomSpawnableEnemyTile()
    {
        var tile = _blocks.Keys.ToList().GetRange(0, 4)[Random.Range(0, 4)];
        return _blocks[tile].GetSpawnPosition();
    }
    
    public Vector3? GetTilePosition(Vector2Int tile) => 
        _blocks.TryGetValue(tile, out var block) ? block.transform.position : null;

    public Vector2Int GetBlockKey(Vector3 position)
    {
        return new Vector2Int(
            Mathf.RoundToInt(position.x),
            Mathf.RoundToInt(position.z)
        );
    }
}

public interface IBoardService
{
    Task Initialize(GameServices services);
}