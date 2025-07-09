using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class EditableErrorsRendererResizer : MonoBehaviour {
    [SerializeField] private float _editableDefaultSize;
    [SerializeField] private RectTransform _toRebuild;

    private void Update() {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_toRebuild);
    }

    public void Resize(Vector2 size) {
        size /= _editableDefaultSize;
        float scale = (size.x + size.y) / 2f;
        scale = Mathf.Max(scale, 1);
        transform.localScale = new Vector2(scale, scale);
    }
}
