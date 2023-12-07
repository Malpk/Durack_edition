using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RoomList : MonoBehaviour
{
    [SerializeField] private RoomPanel _prefab;
    [SerializeField] private RectTransform _content;
    [SerializeField] private GridLayoutGroup _holder;

    private List<RoomPanel> _pool = new List<RoomPanel>();
    private List<RoomPanel> _activeRooms = new List<RoomPanel>();

    public event System.Action<RoomData> OnEnterRoom;

    public void UpdateRoom(RoomData [] datas)
    {
        var active = new List<RoomPanel>();
        foreach (var data in datas)
        {
            active.Add(AddRoom(data));
        }
        while (_activeRooms.Count > 0)
        {
            DeleteRooom(_activeRooms[0]);
        }
        _activeRooms = active;
        UpdateContentCanvas();
    }

    private void Enter(RoomData data)
    {
        OnEnterRoom?.Invoke(data);
    }

    private RoomPanel AddRoom(RoomData id)
    {
        var panel = Create();
        panel.Bind(id);
        return panel;
    }

    private void UpdateContentCanvas()
    {
        var rect = _content.sizeDelta;
        var steap = _holder.cellSize.y + _holder.spacing.y;
        rect.y = steap * (_activeRooms.Count + 1);
        _content.sizeDelta = rect;
    }

    private void DeleteRooom(RoomPanel panel)
    {
        panel.gameObject.SetActive(false);
        panel.OnEnter -= Enter;
        _activeRooms.Remove(panel);
        _pool.Add(panel);
    }

    private RoomPanel Create()
    {
        if (_activeRooms.Count > 0)
        {
            var panel = _activeRooms[0];
            _activeRooms.RemoveAt(0);
            return panel;
        }
        else if (_pool.Count > 0)
        {
            var panel = _pool[0];
            panel.gameObject.SetActive(true);
            panel.OnEnter += Enter;
            _pool.RemoveAt(0);
            return panel;
        }
        else
        {
            var panel = Instantiate(_prefab.gameObject, _holder.transform).
                GetComponent<RoomPanel>();
            panel.OnEnter += Enter;
            return panel;
        }
    }


}
