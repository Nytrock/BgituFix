public class EditableComputerPool : Pool<EditableComputer> {
    private MapEditManager _editManager;
    private MapManager _mapManager;
    private ComputerUIManager _computerUIManager;
    private SelectManager _selectManager;

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI, MapEditManager editManager, SelectManager selectManager) {
        _mapManager = mapManager;
        _computerUIManager = computerUI;
        _editManager = editManager;
        _selectManager = selectManager;
    }

    protected override EditableComputer CreateObject() {
        EditableComputer computer = base.CreateObject();
        computer.SetManagers(_mapManager, _computerUIManager, _editManager, _selectManager);
        return computer;
    }
}
