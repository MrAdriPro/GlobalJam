using UnityEngine;

[CreateAssetMenu(menuName = "PersistentData/new Persistent Data", fileName = "New Persistent Data")]
public class PersistentData : ScriptableObject
{
    [Header("Display")]
    public CustomRes resolution;
    public bool vSync;
    public bool fullScreen;
    public int antialiasing;

    [Header("Audio")]
    public float master;
    public float sounds;
    public float music;

    [Header("Inputs")]
    public int mouseSens;
    public int joystick1Sens;
    public int joystick2Sens;

}

[System.Serializable]
public class CustomRes
{
    public int width;
    public int height;
    public double hz;

    public CustomRes(int _width, int _height, double _hz) 
    {
        width = _width;
        height = _height;
        hz = _hz;

    }
}
