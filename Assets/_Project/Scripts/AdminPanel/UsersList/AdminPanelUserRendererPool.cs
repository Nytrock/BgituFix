public class AdminPanelUserRendererPool : Pool<AdminPanelUserRenderer> {
    private AdminPanelUsersList _usersList;

    public void SetUsersList(AdminPanelUsersList adminPanelUsersList) {
        _usersList = adminPanelUsersList;
    }

    protected override AdminPanelUserRenderer CreateObject() {
        AdminPanelUserRenderer renderer = base.CreateObject();
        renderer.SetUsersList(_usersList);
        return renderer;
    }
}
