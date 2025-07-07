using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapAudience : MapElement<EditableComputer, ComputerData, AudienceData> {
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _sizesTextsMultiplier;
    [SerializeField] private EditableTextSize _widthText;
    [SerializeField] private EditableTextSize _lengthText;

    private bool _isShowingSize;

    public override void Setup(MapData mapData, AudienceData data, float precision) {
        base.Setup(mapData, data, precision);
        ChangeSizeShowState(false);
        UpdateSize();
    }

    public void UpdateData(AudienceData newData) {
        _data = newData;
        UpdateSize();
    }

    public void UpdateContainingComputersPositions() {
        foreach (var computer in _editables) {
            Vector2 computerPosition = computer.Position;
            if (computer.LeftBottom.x < _data.Size.x / -2f)
                computerPosition += new Vector2(_data.Size.x / -2f - computer.LeftBottom.x, 0);
            else if (computer.LeftBottom.y < _data.Size.y / -2f)
                computerPosition += new Vector2(0, _data.Size.y / -2f - computer.LeftBottom.y);
            else if (computer.RightTop.x > _data.Size.x / 2f)
                computerPosition += new Vector2(_data.Size.x / 2f - computer.RightTop.x, 0);
            else if (computer.RightTop.y > _data.Size.y / 2f)
                computerPosition += new Vector2(0, _data.Size.y / 2f - computer.RightTop.y);

            computer.ChangePosition(computerPosition);
        }
    }

    private void Update() {
        if (!_isShowingSize)
            return;

        _widthText.SetSize(_data.Size.x);
        _lengthText.SetSize(_data.Size.y);
    }

    protected override void ChangeSizeShowState(bool newState) {
        _canvas.gameObject.SetActive(newState);
        _isShowingSize = newState;
    }

    public void UpdateSize() {
        _renderer.size = _data.Size;
        _canvas.sizeDelta = _data.Size * (1 / _canvas.localScale.x);
        _cameraSize = Mathf.Max(_data.Size.x / 2f / 16f * 9, _data.Size.y / 2f);
    }

    protected override IEnumerable<ComputerData> GetEditablesData(MapData mapData) {
        return mapData.GetComputersByAudience(_data);
    }

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI,
        ErrorManager errorManager, MapEditManager editManager, SelectManager selectManager) {

        (_pool as EditableComputerPool).SetManagers(mapManager, computerUI, editManager, selectManager);

        errorManager.ErrorAdded += CheckNewError;
        errorManager.ErrorChanged += CheckChangedError;
        errorManager.ErrorDeleted += CheckDeletedError;
    }

    public void CheckNewError(ComputerErrorData errorData) {
        foreach (var computer in _editables)
            computer.CheckNewError(errorData);
    }

    public void CheckChangedError(ComputerErrorData errorData) {
        foreach (var computer in _editables)
            computer.CheckChangedError(errorData);
    }

    public void CheckDeletedError(ComputerErrorData errorData) {
        foreach (var computer in _editables)
            computer.CheckDeletedError(errorData);
    }

    public void Delete() {
        foreach (var computer in _editables)
            _pool.PutObject(computer);
        _editables.Clear();
    }

    protected override ComputerData GenerateEmptyEditableData() {
        int computersCount = _mapData.ComputerDatas.Count();
        return new(_data.Id, computersCount);
    }
}
