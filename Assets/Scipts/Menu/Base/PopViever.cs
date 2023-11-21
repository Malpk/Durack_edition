using UnityEngine;

public class PopViever : MonoBehaviour
{
    private UIMenu _popOpen;

    public void ShowPop(UIMenu pop, Vector2 position)
    {
        pop.transform.position = position;
        ShowPop(pop);
    }

    public void ShowPop(UIMenu pop)
    {
        if (_popOpen)
            _popOpen.Hide();
        _popOpen = pop;
        _popOpen.Show();
    }

    public void HidePop()
    {
        if (_popOpen)
        {
            _popOpen.Hide();
            _popOpen = null;
        }
    }
}
