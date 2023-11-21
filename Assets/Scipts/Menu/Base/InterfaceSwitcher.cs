using UnityEngine;

public class InterfaceSwitcher : MonoBehaviour
{
    [SerializeField] private UIMenu[] _menu;

    private UIMenu _openMenu;

    public void SwitchMenu(MenuType type)
    {
        if (_openMenu)
            _openMenu.Hide();
        if (type != MenuType.None)
        {
            _openMenu = GetMenu(type);
            _openMenu.Show();
        }
    }

    private UIMenu GetMenu(MenuType type)
    {
        foreach (var menu in _menu)
        {
            if (menu.Type == type)
            {
                return menu;
            }
        }
        return null;
    }
}
