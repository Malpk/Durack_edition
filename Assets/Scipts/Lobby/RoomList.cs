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

    public event System.Action<uint, System.Action<string>> OnEnterRoom;

    public void UpdateRoom(uint [] ids)
    {
        var list = GetNewRoom(ids);
        foreach (var id in list)
        {
            AddRoom(id);
        }
    }

    private List<uint> GetNewRoom(uint[] ids)
    {
        var list = new List<uint>();
        list.AddRange(ids);
        var delete = new List<RoomPanel>();
        foreach (var panel in _activeRooms)
        {
            if (list.Contains(panel.ID))
            {
                list.Remove(panel.ID);
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
        OnEnterRoom?.Invoke(id, GetAnswer);
    }

    private void GetAnswer(string answer)
    {
        Debug.Log(answer);
    }

    private void AddRoom(uint id)
    {
        var panel = Create();
        panel.Bind(id);
        panel.OnEnter += Enter;
        _activeRooms.Add(panel);
        UpdateContentCanvas();
    }

    private void DeleteRooom(uint id)
    {
        var panel = GetPanel(id);
        DeleteRooom(panel);
        UpdateContentCanvas();
    }

    private void UpdateContentCanvas()
    {
        var rect = _content.rect;
        rect.height = _holder.cellSize.y * (_activeRooms.Count + 1);
        _content.rect.Set(rect.x, rect.y, rect.width, rect.height);
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
