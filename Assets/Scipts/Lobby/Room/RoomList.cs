using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RoomList : MonoBehaviour
{
    [SerializeField] private string _requestKey;
    [SerializeField] private string _answerKey;
    [Header("Reference")]
    [SerializeField] private RoomPanel _prefab;
    [SerializeField] private RectTransform _content;
    [SerializeField] private GridLayoutGroup _holder;

    private List<RoomPanel> _pool = new List<RoomPanel>();
    private List<RoomPanel> _activeRooms = new List<RoomPanel>();

    public event System.Action<uint> OnEnterRoom;

    public void UpdateRoom(RoomData [] datas)
    {
        var list = GetNewRoom(datas);
        foreach (var data in list)
        {
            AddRoom(data);
        }
    }

    private List<RoomData> GetNewRoom(RoomData[] ids)
    {
        var list = new List<RoomData>();
        list.AddRange(ids);
        var delete = new List<RoomPanel>();
        foreach (var panel in _activeRooms)
        {
            if (list.Contains(panel.Data))
            {
                list.Remove(panel.Data);
            }
            else
            {
                delete.Add(panel);
            }
        }
        foreach (var panel in delete)
        {
            DeleteRooom(panel);
        }
        return list;
    }

    private void Enter(uint id)
    {
        OnEnterRoom?.Invoke(id);
    }

    private void AddRoom(RoomData id)
    {
        var panel = Create();
        panel.Bind(id);
        panel.OnEnter += Enter;
        _activeRooms.Add(panel);
        UpdateContentCanvas();
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
        if (panel)
        {
            panel.OnEnter -= Enter;
            _activeRooms.Remove(panel);
            _pool.Add(panel);
        }
    }

    private RoomPanel GetPanel(uint id)
    {
        foreach (var room in _activeRooms)
        {
            if (id == room.ID)
                return room;
        }
        return null;
    }

    private RoomPanel Create()
    {
        if (_pool.Count > 0)
        {
            var panel = _pool[0];
            _pool.RemoveAt(0);
            return panel;
        }
        else
        {
            return Instantiate(_prefab.gameObject, _holder.transform).
                GetComponent<RoomPanel>();
        }
    }


}
