using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapAudience : MapElement<EditableComputer, AudienceData> {
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private float _sizesTextsMultiplier;
    [SerializeField] private EditableTextSize _widthText;
    [SerializeField] private EditableTextSize _lengthText;

    private bool _isShowingSize;

    public override void Setup(MapData mapData, IdData data, float precision) {
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

    public override BaseEditable GenerateEditable(EditableData data, bool neeedOverlapCheck = false) {
        _mapData.AddComputer(data as ComputerData);
        return base.GenerateEditable(data, neeedOverlapCheck);
    }

    public void UpdateSize() {
        _renderer.size = _data.Size;
        _canvas.sizeDelta = _data.Size * (1 / _canvas.localScale.x);
        _cameraSize = Mathf.Max(_data.Size.x / 2f / 16f * 9, _data.Size.y / 2f);
    }

    protected override IEnumerable<EditableData> GetEditablesData(MapData mapData) {
        return mapData.GetComputersByAudienceId(_data.Id);
    }

    public override void Inject(ObjectResolver resolver) {
        _pool.SetResolver(resolver);

        var errorManager = resolver.GetInstance<ErrorManager>();
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
        foreach (EditableComputer computer in _editables.Cast<EditableComputer>())
            _pool.PutObject(computer);
        _editables.Clear();
    }

    protected override EditableData GenerateEmptyEditableData() {
        int computersCount = _mapData.ComputerDatas.Count();
        return new ComputerData(_data.Id, computersCount);
    }

    public override void GetBounds(out Vector2 leftDown, out Vector2 rightUp) {
        leftDown = -_data.Size / 2f;
        rightUp = _data.Size / 2f;
    }
}
