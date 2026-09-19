using UnityEngine;

public class ResolverPool<T> : Pool<T> where T : MonoBehaviour, IResolvable {
    [SerializeField] private ObjectResolver _resolver;

    public void SetResolver(ObjectResolver resolver) {
        _resolver = resolver;
    }

    protected override T CreateObject() {
        T newObject = base.CreateObject();
        newObject.Inject(_resolver);
        return newObject;
    }
}
