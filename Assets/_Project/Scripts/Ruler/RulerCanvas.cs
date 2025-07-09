using UnityEngine;

public class RulerCanvas : MonoBehaviour {
    [SerializeField] private GameObject _arrowsContainer;
    [SerializeField] private RulerArrow _horizontalArrow;
    [SerializeField] private RulerArrow _verticalArrow;

    public void SetArrows(float left, float right, float top, float bottom) {
        _horizontalArrow.SetSize(left, right);
        _verticalArrow.SetSize(bottom, top);
    }

    public void ChangeState(bool isRulerInside) {
        _arrowsContainer.SetActive(isRulerInside);
    }
}
