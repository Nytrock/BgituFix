using UnityEngine;

public abstract class BaseMapElement : MonoBehaviour {
    protected float _cameraSize;

    public float CameraSize => _cameraSize;

    public virtual void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }
}
