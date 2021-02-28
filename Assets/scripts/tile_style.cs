using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class tile
{
    public int number;
    public Color32 tilecolor;
    public Color32 textcolor;
}
public class tile_style : MonoBehaviour
{
    public static tile_style ins;
    public tile[] tilestyles;
    // Start is called before the first frame update
    private void Awake()
    {
        ins = this;
    }
}
