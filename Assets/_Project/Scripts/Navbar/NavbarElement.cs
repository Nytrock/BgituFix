using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NavbarElement : MonoBehaviour {
    [SerializeField] private StateMachine _element;
    [SerializeField] private GameObject _underline;

    public Button Button => GetComponent<Button>();

    public void ChangeState(bool newState) {
        _element.ChangeState(newState);
        _underline.SetActive(newState);
    }
}
