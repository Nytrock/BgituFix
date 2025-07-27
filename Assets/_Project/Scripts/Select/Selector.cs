using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(SpriteRenderer))]
public class Selector : MonoBehaviour {
    [SerializeField] private MapEditManager _editManager;
    [SerializeField] private List<BaseEditable> _editCandidates = new();
    [SerializeField] private List<BaseEditable> _editParticipants = new();

    private BoxCollider2D _collider;
    private SpriteRenderer _renderer;

    private bool _isActive;
    private Vector2 _startPosition;

    public bool IsActive => _isActive;

    private void Awake() {
        _collider = GetComponent<BoxCollider2D>();
        _renderer = GetComponent<SpriteRenderer>();
        ChangeState(false);
    }

    public void ChangeState(bool isActive) {
        _isActive = isActive;
        gameObject.SetActive(isActive);

        if (isActive) {
            _startPosition = CameraManager.LocalMousePosition;
        } else {
            SetSizeAndPosition(0, 0, 0, 0);
            _editCandidates.Clear();
            _editParticipants.Clear();
        }
    }

    private void Update() {
        if (!_isActive)
            return;

        UpdateSizeAndPosition();
        CheckEditables();
    }

    private void UpdateSizeAndPosition() {
        Vector2 mousePostion = CameraManager.LocalMousePosition;
        float left = Mathf.Min(_startPosition.x, mousePostion.x);
        float right = Mathf.Max(_startPosition.x, mousePostion.x);
        float bottom = Mathf.Min(_startPosition.y, mousePostion.y);
        float top = Mathf.Max(_startPosition.y, mousePostion.y);
        SetSizeAndPosition(right - left, top - bottom, (left + right) / 2f, (bottom + top) / 2f);
    }

    private void SetSizeAndPosition(float width, float height, float x, float y) {
        _collider.size = new(width, height);
        _renderer.size = _collider.size;
        transform.position = new(x, y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.TryGetComponent(out EditableActivator editableActivator))
            return;

        AddEditable(editableActivator.Editable);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (!_isActive)
            return;

        if (!other.TryGetComponent(out EditableActivator editableActivator))
            return;

        RemoveEditable(editableActivator.Editable);
    }

    private void AddEditable(BaseEditable editable) {
        _editCandidates.Add(editable);
    }

    private void RemoveEditable(BaseEditable editable) {
        if (_editCandidates.Contains(editable)) {
            _editCandidates.Remove(editable);
        } else {
            _editParticipants.Remove(editable);
            _editManager.ChangeSelectStateOfAdditionalEditable(editable);
        }
    }

    private void CheckEditables() {
        Vector2 bottomLeft = (Vector2)transform.position - _renderer.size / 2f;
        Vector2 topRight = (Vector2)transform.position + _renderer.size / 2f;

        List<BaseEditable> temp = new();
        foreach (var candidate in _editCandidates) {
            Vector2 candidateBottomLeft = (Vector2)candidate.transform.position - candidate.Size / 2f;
            Vector2 candidateTopRight = (Vector2)candidate.transform.position + candidate.Size / 2f;

            if (EditorUtils.RectContainsRect(bottomLeft, topRight, candidateBottomLeft, candidateTopRight))
                temp.Add(candidate);
        }

        foreach (var newParticipant in temp)
            MoveEditableToParticipants(newParticipant);
        temp.Clear();

        foreach (var participant in _editParticipants) {
            Vector2 participantBottomLeft = (Vector2)participant.transform.position - participant.Size / 2f;
            Vector2 participantTopRight = (Vector2)participant.transform.position + participant.Size / 2f;

            if (!EditorUtils.RectContainsRect(bottomLeft, topRight, participantBottomLeft, participantTopRight))
                temp.Add(participant);
        }

        foreach (var newCandidate in temp)
            MoveEditableToCandidates(newCandidate);
    }

    private void MoveEditableToParticipants(BaseEditable editable) {
        _editCandidates.Remove(editable);
        _editManager.ChangeSelectStateOfAdditionalEditable(editable);
        _editParticipants.Add(editable);
    }

    private void MoveEditableToCandidates(BaseEditable editable) {
        _editParticipants.Remove(editable);
        _editManager.ChangeSelectStateOfAdditionalEditable(editable);
        _editCandidates.Add(editable);
    }
}
