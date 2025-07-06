using UnityEngine;

public abstract class ThemeElement<T> : MonoBehaviour {
    [SerializeField] protected ThemeColorVariable _variable;

    protected T _element;

    private void Start() {
        GetComponents();
        ThemeManager.Instance.IsModeChanged += SetColor;
        SetColor();
    }

    protected abstract void SetColor();

    protected void GetComponents() {
        _element = GetComponent<T>();
    }
}
