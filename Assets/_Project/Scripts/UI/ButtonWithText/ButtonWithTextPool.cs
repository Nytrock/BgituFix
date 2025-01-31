public class ButtonWithTextPool : Pool<ButtonWithText> {
    public override ButtonWithText GetObject() {
        ButtonWithText button = base.GetObject();
        button.transform.SetAsLastSibling();
        return button;
    }

    public override void PutObject(ButtonWithText obj) {
        obj.onClick.RemoveAllListeners();
        base.PutObject(obj);
    }
}
