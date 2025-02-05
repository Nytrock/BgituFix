using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapAudience : MapElement<EditableComputer, ComputerData, AudienceData> {
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private TextMeshProUGUI _widthText;
    [SerializeField] private TextMeshProUGUI _lengthText;

    private bool _isShowingSize;

    public override int Id => _data.Id;

    public override void Setup(MapData mapData, AudienceData data) {
        base.Setup(mapData, data);
        ChangeSizeShowState(false);
        UpdateSize();
    }

    private void Update() {
        if (!_isShowingSize)
            return;

        _widthText.text = _data.Size.x.ToString() + Units.SIZE_UNIT;
        _lengthText.text = _data.Size.y.ToString() + Units.SIZE_UNIT;
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
        ErrorManager errorManager, MapEditManager editManager) {

        (_pool as EditableComputerPool).SetManagers(mapManager, computerUI, editManager);

        editManager.EditableChanged += UpdateShowingSize;
        editManager.EditStateChanged += UpdateShowingSize;
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
}
