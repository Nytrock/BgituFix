using UnityEngine;

public abstract class Editable<TData> : MonoBehaviour
    where TData : EditableData {

    protected TData _data;

    public virtual void Setup(TData data) {

    }
}
