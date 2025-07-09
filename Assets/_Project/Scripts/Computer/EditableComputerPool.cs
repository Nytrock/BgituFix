public class EditableComputerPool : Pool<EditableComputer> {
    private MapManager _mapManager;
    private ComputerUIManager _computerUIManager;
    private SelectManager _selectManager;

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI, SelectManager selectManager) {
        _mapManager = mapManager;
        _computerUIManager = computerUI;
        _selectManager = selectManager;
    }

    protected override EditableComputer CreateObject() {
        EditableComputer computer = base.CreateObject();
        computer.SetManagers(_mapManager, _computerUIManager, _selectManager);
        return computer;
    }
}
