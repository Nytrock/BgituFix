using UnityEngine;

public abstract class ThemeElement<T> : MonoBehaviour where T : Object {
    [SerializeField] protected ThemeColorVariable _variable;

    protected T _element;
    protected string _name;

    private void Start() {
        GetComponents();
        ThemeManager.Instance.IsModeChanged += SetColor;
        SetColor();
    }

    private void OnDestroy() {
        ThemeManager.Instance.IsModeChanged -= SetColor;
    }

    protected abstract void SetColor();

    protected void GetComponents() {
        _element = GetComponent<T>();
    }
}
