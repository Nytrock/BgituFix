using UnityEngine;

public abstract class MapElement<TEditable, TData> : BaseMapElement where TEditable : BaseEditable where TData : IdData {
    [SerializeField] protected ResolverPool<TEditable> _pool;

    protected TData _data;
    public TData Data => _data;

    public override void Setup(MapData mapData, IdData data, float precision) {
        base.Setup(mapData, data, precision);
        _data = data as TData;
    }

    public override void DeleteEditable(BaseEditable editable) {
        base.DeleteEditable(editable);
        _pool.PutObject(editable as TEditable);
    }

    protected override BaseEditable GetNewEditable() {
        return _pool.GetObject();
    }
}
