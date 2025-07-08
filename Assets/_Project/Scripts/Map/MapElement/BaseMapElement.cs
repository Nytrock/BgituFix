using UnityEngine;

public abstract class BaseMapElement : MonoBehaviour {

    protected float _cameraSize;
    protected float _precision;

    public float CameraSize => _cameraSize;
    public float Precision => _precision;

    public void UpdateShowingSize(bool isShow) {
        if (!gameObject.activeSelf)
            return;

        ChangeSizeShowState(isShow);
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    protected abstract void ChangeSizeShowState(bool newState);
}
