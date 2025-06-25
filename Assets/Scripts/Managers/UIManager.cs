using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private DefenceItemView defenceItemView;
    [SerializeField] private FinishPanel _finishPanel;
    
    private GameServices _services;
    
    public async Task InitializeUI(GameServices services)
    {
        _services = services;
        
        var levelData = _services.ResolveData<ILevelRuntimeData, LevelRuntimeData>();
        var levelDefenceItems = levelData.DefenceItems();
        
        GameManager.Instance.CustomEvent.AddCustomEventListener<GameEnd>(HandleEnd);
        
        await defenceItemView.Initialize(levelDefenceItems);
        
        transform.SetParent(null);
    }
    
    private void OnSuccess()
    {
        var sceneService = _services.ResolveService<ISceneService, SceneService>();
        
        _finishPanel.gameObject.SetActive(true);
        _finishPanel.ShowSuccessPanel(sceneService.ChangeScene);
    }

    private void OnFailed()
    {
        var sceneService = _services.ResolveService<ISceneService, SceneService>();
        
        _finishPanel.gameObject.SetActive(true);
        _finishPanel.ShowFailedPanel(sceneService.ChangeScene);
    }

    private void HandleEnd(GameEnd e)
    {
        if (e.win)
        {
            OnSuccess();
        }
        else
        {
            OnFailed();
        }
    }
}