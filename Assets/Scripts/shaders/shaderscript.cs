using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class shaderscript : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap tilemap2;
    public Material highlightMaterial;
    public Sprite highTile;
    public TileBase newTile;
    public TileBase newerTiler;

    int movement = 0;
    public static shaderscript Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

    }
}
    /*

    public void HighlightTiles(List<Vector3> tilePositions)
    {
        DeHighLight();
            foreach (Vector3 pos in tilePositions)
        {
            //tilemap.SetTile(tilemap.WorldToCell(pos), newTile);
        }
        
    }

    public void HightLightTile(Vector3Int tilePosition, int width, int height) {

        //DeHightLightTile();


        if (width < 2)
        {
            tilemap2.SetTile((tilePosition), newerTiler);
        }
        else
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3Int pos = tilePosition;
                    pos.x += i;
                    pos.y += j;
                    tilemap2.SetTile(pos, newerTiler);
                }

            }
        
    }

    public void DeHightLightTile()
    {

        //if (tilePosition.x > tilePosition.x + movement || tilePosition.y > tilePosition.y + movement || tilePosition.x < tilePosition.x - movement || tilePosition.y < tilePosition.y - movement)
        //  return;
       // tilemap2.ClearAllTiles();
        /*
        if (width < 2)
        {
            tilemap2.SetTile((tilePosition), newTile);
        }
        else
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3Int pos = tilePosition;
                    pos.x += i;
                    pos.y += j;
                    tilemap2.SetTile(pos, newTile);
                }

            }
        
    }


    public void DeHighLight()
    {
        
            //tilemap.ClearAllTiles();
        //tilemap2.ClearAllTiles();
        
    }
}
    */