using UnityEngine;

public class ComputerErrorUIPool : Pool<ComputerErrorUI> {
    [SerializeField] private ComputerErrorsUI _errorManager;

    protected override ComputerErrorUI CreateObject() {
        ComputerErrorUI errorUI = base.CreateObject();
        errorUI.SetErrorManager(_errorManager);
        return errorUI;
    }
}
