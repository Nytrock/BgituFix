public class EditableComputerPool : Pool<EditableComputer> {
    private MapEditManager _editManager;
    private MapManager _mapManager;
    private ComputerUIManager _computerUIManager;

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI, MapEditManager editManager) {
        _mapManager = mapManager;
        _computerUIManager = computerUI;
        _editManager = editManager;
    }

    protected override EditableComputer CreateObject() {
        EditableComputer computer = base.CreateObject();
        computer.SetManagers(_mapManager, _computerUIManager, _editManager);
        return computer;
    }
}
