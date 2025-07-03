using UnityEngine;

public abstract class BaseMapElement : MonoBehaviour {

    protected float _cameraSize;
    public float CameraSize => _cameraSize;

    public void UpdateShowingSize(bool isEdit) {
        if (!gameObject.activeSelf)
            return;

        ChangeSizeShowState(isEdit);
    }

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }

    protected abstract void ChangeSizeShowState(bool newState);
}
