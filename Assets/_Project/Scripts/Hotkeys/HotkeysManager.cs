using UnityEngine;

public class HotkeysManager : MonoBehaviour {
    [SerializeField] private MapEditManager _editManager;
    [SerializeField] private SelectManager _selectManager;
    [SerializeField] private RulerManager _rulerManager;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private StateMachine _helpPanel;

    private void Update() {
        if (_selectManager.IsSelectorActive || !_editManager.IsEdit)
            return;

        if (Input.GetKeyDown(KeyCode.Delete))
            _editManager.Delete();

        if (!Input.GetKey(KeyCode.LeftControl))
            return;

        if (Input.GetKeyDown(KeyCode.C))
            _editManager.Copy();
        else if (Input.GetKeyDown(KeyCode.V))
            _editManager.Paste();
        else if (Input.GetKeyDown(KeyCode.S))
            _editManager.SaveChangesWithoutEndingEdit();
        else if (Input.GetKeyDown(KeyCode.N))
            _editManager.CreateEmptyEditable();
        else if (Input.GetKeyDown(KeyCode.G))
            _gridManager.ChangeState(!_gridManager.IsActive);
        else if (Input.GetKeyDown(KeyCode.R))
            _rulerManager.ChangeState(!_rulerManager.IsActive);
        else if (Input.GetKeyDown(KeyCode.H))
            _helpPanel.ChangeState();
    }
}
