using UnityEngine;

public class ComputerErrorFormUI : MonoBehaviour {
    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
    }
}
