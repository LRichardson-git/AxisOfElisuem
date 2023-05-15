using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TileMapper : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap tilemap;

    public static TileMapper Instance { get; private set; }
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        Instance = this;
    }
}
