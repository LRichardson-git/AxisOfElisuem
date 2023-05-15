using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapGrid : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap grid;

    public Terrain[] Tiles_Biome;
    [System.Serializable]
    public struct Terrain
    {
        public string name;
        public float height;
        public Tile TileSprite;
    }
  
    //grass, sand, water, mountain
    void Start()
    {
        

        
    }


    public void UpdateTileMap(float[,] Map_Noise, int Height, int Width)
    {


        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float CurrentHeight = Map_Noise[x, y];

                //loop through each biome to see what this current height falls within
                for (int i = 0; i < Tiles_Biome.Length; i++)
                {
                    if (CurrentHeight <= Tiles_Biome[i].height) //xD
                    {
                        grid.SetTile(new Vector3Int(x, y, 0), Tiles_Biome[i].TileSprite);
                        //Debug.Log(Tiles_Biome[i].name);
                        //found biome so can move one
                        break;
                    }
                }
            }
        }


        }





    // Update is called once per frame
    void Update()
    {
        
    }
}
