using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Map_Generation : MonoBehaviour
{
    public static Map_Generation mapGen;

    //Values for map generation
    public int Width;
    public int Height;
    public float Scale_Noise;

    //Disable or enable these noise types
    public bool Perlin_Noise;
    public bool value_noise;
    public bool Simplex_noise;

    public int randomsA = 100;

    //Type of map to draw

    public static Map_Generation Instance { get; private set; }

    public Draw_Mode DrawMap;

    //type of noise to use as the base noise map
  
    public Noise_Type Base_NoiseType;

    //Settings that affect overall outcome of terrain
    [Space(25)] public int Octaves;
    [Range(0, 1)] public float Amplitude;
    public float Frequency;
    public int Seed;
    public Vector2 OffSet; //move around map
    public float MeshHeight;
    public AnimationCurve MeshHeightCurve;


    [Space(25)]
    //enable showing bulding location and spawning them
    public bool erosion = false;
    public int Rain_iterations = 30000;
    public double FlatLand = 0.0008;

    public bool random = false;
    public bool Auto_Update;
    float[,] Map_Noise;
    Color[] Map_Colour;

    //Used in editor to create the colours of the map
    public Terrain[] Biomes;

    [System.Serializable]
    public struct Terrain //Struct for sorting colours on map
    {
        public string name;
        public float height;
        public Color colour;
    }

    public void Awake()
    {

        if (mapGen != null)
            Destroy(mapGen);
        else
            mapGen = this;

        Instance = this;
    }

    public float[,] getMapNoise() { return Map_Noise; }


    public void Generate_Map()
    {

        //Set base type of map
        int type = 1;
        if (Base_NoiseType == Noise_Type.Value)
            type = 2;
        else if (Base_NoiseType == Noise_Type.simplex)
            type = 3;


        if (random == true)
        {
            System.Random Ran_Seed = new System.Random();

            if (Ran_Seed.Next(0, 2) == 1){erosion = true;}
            else
                erosion = false;
        }

        //Generate a noise map
        Map_Noise = Noise_Maps.GenNoiseMap(Width, Height, Seed, Scale_Noise, Octaves, Amplitude, Frequency, OffSet,
            Perlin_Noise, value_noise, Simplex_noise, type);

        //colouring the map
        Map_Colour = new Color[Width * Height];




        if (erosion == true)
        {
            erode();
        }


        ColourMap();


        
        //Create references to scripts that generate buildings, map and vegation
        Display_Map Display = FindObjectOfType<Display_Map>();

        if (DrawMap == Draw_Mode.NoiseMap)
        {
            Display.Drawtextures(Textures.textureHeightMap(Map_Noise));
        }

        //just colour map
        if (DrawMap == Draw_Mode.ColourMap)
            Display.Drawtextures(Textures.TextureFromMap(Map_Colour, Width, Height));

        //Mesh with possible buildings and vegation
        else if (DrawMap == Draw_Mode.Mesh)
        {
            //Display script activates creation of mesh
            Display.DrawMesh(MapMeshGenertion.GenerateMeshTerrain(Map_Noise, MeshHeight, MeshHeightCurve),
                Textures.TextureFromMap(Map_Colour, Width, Height));
        }

    }


    public static Texture2D NoiseMapToTexture(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        Texture2D texture = new Texture2D(width, height);

        Color[] colorArray = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colorArray[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        texture.SetPixels(colorArray);
        texture.Apply();
        return texture;
    }

    //make sure editor values are valide
    private void OnValidate()
    {
        //Make sure editor values are not invalid or simulation breaking
        if (Width < 1)
            Width = 1;

        if (Height < 1)
            Height = 1;

        if (Frequency < 1)
            Frequency = 1;

        if (Octaves < 1)
            Octaves = 1;

        if (Perlin_Noise == false && Simplex_noise == false && value_noise == false)
            Perlin_Noise = true;

        if (Octaves > 20)
            Octaves = 20;

        if (Height != Width)
            Height = Width;
    }

    void erode()
    {
        float[] heightmap = new float[Width * Height];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                heightmap[y * Width + x] = Map_Noise[x, y];
            }
        }

        Erosion lol = FindObjectOfType<Erosion>();

        if (random == true)
        {
            System.Random Ran_Seed = new System.Random();

            lol.inertia = (float) Ran_Seed.Next(0, 100) / 100;
            lol.erosionRadius = (Ran_Seed).Next(3, 15);
            lol.sediment_amount_capicty = (float) (Ran_Seed).Next(1, 140) / 100;
            lol.sediment_amount_capicty_min = (float) (Ran_Seed).Next(0, 30) / 10;
            lol.disolve_rate = (float) (Ran_Seed).Next(0, 100) / 100;
            lol.deposit = (float) (Ran_Seed).Next(0, 30) / 10;
            lol.evaportion_rate = (float) (Ran_Seed).Next(0, 30) / 10;
            lol.gravity = (Ran_Seed).Next(1, 50);
            lol.max_DropletLife = (float) (Ran_Seed).Next(10, 50);
            lol.rain_rate = (Ran_Seed).Next(1, 10);
            lol.inital_speed = (Ran_Seed).Next(1, 10);
            lol.erodeSpeed = (float) (Ran_Seed).Next(1, 100) / 100;
        }


        lol.erosion(Seed, heightmap, Rain_iterations, Width);

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Map_Noise[x, y] = heightmap[y * Width + x];
            }
        }
    }


    void ColourMap()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float CurrentHeight = Map_Noise[x, y];

                //loop through each biome to see what this current height falls within
                for (int i = 0; i < Biomes.Length; i++)
                {
                    if (CurrentHeight <= Biomes[i].height) //xD
                    {
                        Map_Colour[y * Width + x] = Biomes[i].colour;
                        //found biome so can move one
                        break;
                    }
                }
            }
        }
    }



 
    public void randomgen()
    {
        System.Random Ran_Seed = new System.Random();

        int xd = Ran_Seed.Next(1, 3);

        if (xd == 1)
            Perlin_Noise = true;
        else
            Perlin_Noise = false;

        xd = Ran_Seed.Next(3, 5);

        if (xd == 3)
            value_noise = true;
        else
            value_noise = false;

        xd = Ran_Seed.Next(4, 6);

        if (xd == 4)
            Simplex_noise = true;
        else
            Simplex_noise = false;


        xd = Ran_Seed.Next(1, 4);
        Rain_iterations = (Ran_Seed).Next(20000, 65000);
        switch (xd)
        {
            case 1:
                Base_NoiseType = Noise_Type.Perlin;

                break;

            case 2:
                Base_NoiseType = Noise_Type.simplex;

                break;

            case 3:
                Base_NoiseType = Noise_Type.Value;
                break;
            default:
                break;
        }

        Amplitude = (float) Ran_Seed.Next(15, 80) / 100;
        Frequency = (float) Ran_Seed.Next(5, 25) / 10;

        Generate_Map();
    }
}