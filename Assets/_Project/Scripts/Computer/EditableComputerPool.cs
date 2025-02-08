public class EditableComputerPool : Pool<EditableComputer> {
    private MapManager _mapManager;
    private ComputerUIManager _computerUIManager;

    public void SetManagers(MapManager mapManager, ComputerUIManager computerUI) {
        _mapManager = mapManager;
        _computerUIManager = computerUI;
    }

    protected override EditableComputer CreateObject() {
        EditableComputer computer = base.CreateObject();
        computer.SetManagers(_mapManager, _computerUIManager);
        return computer;
    }
}
