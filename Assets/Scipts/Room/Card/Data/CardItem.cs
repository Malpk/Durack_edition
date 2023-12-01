using UnityEngine;

[System.Serializable]
public class CardItem
{
    public string Path;
    public ENominal Nominal;

    private string _pathLoad;
    private Sprite _content;

    public Sprite GetSprite(string path)
    {
        var fullPath = path + "/" +  Path;
        if (fullPath == _pathLoad)
            return _content;
        _pathLoad = fullPath;
        Resources.UnloadAsset(_content);
        _content = Resources.Load<Sprite>(_pathLoad);
        return _content;
    }
}
