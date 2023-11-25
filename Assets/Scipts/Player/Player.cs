using UnityEngine;
using Client;

public class Player : MonoBehaviour
{
    [SerializeField] private int _chips;
    [SerializeField] private UserData _data;

    public int Chips
    {
        get 
        {
            return _chips;
        }
        private set
        {
            _chips = value;
            OnUpdateChips?.Invoke(_chips);
        }
    }
    public UserData Data => _data;

    public event System.Action<int> OnUpdateChips;

    public void BindPlayer(UserData data)
    {
        _data = data;
    }

    public void SetChip(ClientData data)
    {
        Chips = data.Chips;
    }
}
