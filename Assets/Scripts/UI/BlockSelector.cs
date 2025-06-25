using System;
using System.Threading.Tasks;
using CustomEvent;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{
    private bool _isActive;
    private DefenceItemType _selectedType;

    public Task Initialize()
    {
        _isActive = false;
        _selectedType = DefenceItemType.None;
        
        GameManager.Instance.CustomEvent.AddCustomEventListener<SelectionStarted>(OnSelectionStarted);
        
        return Task.CompletedTask;
    }

    private void OnSelectionStarted(SelectionStarted e)
    {
        _isActive = true;
        _selectedType = e.Type;
    }

    private void Update()
    {
        if (!_isActive || !Input.GetMouseButtonDown(0)) return;
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var gName = hit.collider.gameObject.name;
            var str = gName.Split(',');
            var tile = new Vector2Int(Convert.ToInt16(str[0]), Convert.ToInt16(str[1]));
            
            GameManager.Instance.CustomEvent.InvokeCustomEvent(new SelectionCompleted()
            {
                Tile = tile,
                Type = _selectedType
            });
        }
        else
        {
            GameManager.Instance.CustomEvent.InvokeCustomEvent(new SelectionFailed()
            {
                Type = _selectedType
            });
        }
        
        _isActive = false;
        _selectedType = DefenceItemType.None;
    }
}