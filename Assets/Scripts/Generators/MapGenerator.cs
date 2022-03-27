using System;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Materials;
using Assets.Scripts.Models;
using Assets.Scripts.UI;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    private int _randomFillPercent;
    private int _width;
    private int _height;
    private string _seed;
    private bool _useRandomSeed;
    private int _chunkSize;

    private Cell[,] _map;
    private Chunk[,] _chunks;
    private bool _isUpdated = true;
    [HideInInspector]
    public uint SelectedMaterial;
    public Texture2D cursor;

    private Texture2D _texture;
    [SerializeField]
    private RawImage _image;
    public RenderTexture renderTexture;
    Vector4[] pixels;

    [SerializeField]
    ComputeShader computeShader;
    private ComputeBuffer buffer;

    private MaterialController _controller;

    private static Material[] _palette;
    public static Material[] Palette => _palette;
    public static Action<Action<uint>> OnGameLoaded;
    private void Start()
    {
        LoadMapSettings();
        CreatePalette();
        _controller = new MaterialController();
        renderTexture = new RenderTexture(_width, _height, 24);
        renderTexture.enableRandomWrite = true;
        renderTexture.Create();
        _chunks = new Chunk[_chunkSize, _chunkSize];
        GenerateChunks();

        _texture = new Texture2D(_width, _height);
        _texture.filterMode = FilterMode.Point;
        GetComponent<Renderer>().material.mainTexture = _texture;
        _image.rectTransform.sizeDelta = new Vector2(_width, _height);
        _image.texture = _texture;
        GenerateMap();
        SelectedMaterial = 0;
        Vector2 cursorOffset = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(cursor, cursorOffset, CursorMode.Auto);
        for (int i = 0; i < _map.GetLength(1); i++)
        {
            for (int j = 0; j < _map.GetLength(0); j++)
            {
                _map[i, j].GetAllNeighbours();
                Color c = _controller.GetColor(_map[j, i]);
                pixels[j * _width + i] = new Vector4(c.r, c.g, c.b, c.a);
            }
        }

        int stride = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Vector4));
        buffer = new ComputeBuffer(_width * _height, stride, ComputeBufferType.Default);
        GenerateView();
        computeShader.SetTexture(0, "Result", renderTexture);
        computeShader.SetBuffer(0, "pixels", buffer);
        computeShader.SetInt("resolution", _width);
        computeShader.SetInt("Width", _width);
        computeShader.SetInt("Height", _height);
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
        renderTexture.filterMode = FilterMode.Point;
        _image.texture = renderTexture;
        Vector3[] v = new Vector3[4];
        _image.rectTransform.GetWorldCorners(v);
        foreach (Vector3 corner in v)
        {
            Debug.Log(corner);
        }
        OnGameLoaded?.Invoke(OnMaterialSelected);
    }

    private void GenerateView()
    {
        Vector2 v = ConvertScreenPositionToMapPosition(new Vector2(0, 0));
        Vector2 end = ConvertScreenPositionToMapPosition(new Vector2(Screen.width, Screen.height));
        int width = Mathf.Min((int)end.x - (int)v.x, _width);
        int height = Mathf.Min((int)end.y - (int)v.y, _height);

        for (int i = 0; i < width; i++)
        {
            buffer.SetData(pixels, ((int)v.x + i) * _width + (int)v.y, ((int)v.x + i) * _width + (int)v.y, height);
        }
    }

    private void CreatePalette()
    {
        _palette = new Material[10];
        _palette[0] = new Material(0, Color.black); // empty
        _palette[1] = new Material(1, Color.grey, 10000f); // solid
        _palette[2] = new Material(2, Color.blue, 1000f); // liquid
        _palette[3] = new Material(3, Color.yellow, 1520f); // sand
        _palette[4] = new Material(4, new Color(0f, 0f, 0f, 0.6f), 1.2f, 0); // smoke
        _palette[5] = new Material(5, Color.clear, 1.2f, 0); // steam
        _palette[6] = new Material(6, new Color(0f, 0f, 0.19f), 1800f); // obsidian
        _palette[7] = new Material(7, new Color(0.39f, 0.24f, 0.05f), 1800f, -1, true); // wood
        _palette[8] = new Material(8, new Color(0.5f, 0, 0f), 0); // lava
        _palette[9] = new Material(9, Color.red, 1000f, 0); // fire
    }
    private void OnMaterialSelected(uint material)
    {
        SelectedMaterial = material;
    }

    private void GenerateChunks()
    {

        for (int i = 0; i < _chunks.GetLength(0); i++)
        {
            for (int j = 0; j < _chunks.GetLength(1); j++)
            {
                int maxX = (i != _chunks.GetLength(0) - 1) ? (_width / _chunkSize + (_width / _chunkSize) * i) : _width;
                int maxY = 0;
                if (j != _chunks.GetLength(1) - 1)
                {
                    maxY = (_height / _chunkSize + (_height / _chunkSize) * j);
                }
                else
                {
                    maxY = _height;
                }
                int minX = _width / _chunkSize * i;
                int minY = _height / _chunkSize * j;
                _chunks[i, j] = new Chunk(maxX - 1, maxY - 1, minX, minY, i, j, _chunks);
            }
        }
    }


    public Vector2 ConvertScreenPositionToMapPosition(Vector2 screenPosition)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_image.rectTransform, screenPosition, Camera.main, out pos);

        int pX = Mathf.Clamp(0, (int)(((pos.x - _image.rectTransform.rect.x) * _texture.width) / _image.rectTransform.rect.width), _texture.width);
        int pY = Mathf.Clamp(0, (int)(((pos.y - _image.rectTransform.rect.y) * _texture.height) / _image.rectTransform.rect.height), _texture.height);

        return new Vector2(pX, pY);
    }

    private void FixedUpdate()
    {
        Vector2 start = ConvertScreenPositionToMapPosition(new Vector2(0, 0));
        Chunk startChunk = GetChunk((int)start.x, (int)start.y);
        Vector2 end = ConvertScreenPositionToMapPosition(new Vector2(Screen.width, Screen.height));
        Chunk endChunk = GetChunk((int)end.x, (int)end.y);

        for (int i = startChunk._x; i <= endChunk._x; i++)
        {
            for (int j = startChunk._y; j <= endChunk._y; j++)
            {
                UpdateMap(_chunks[i, j]);
            }
        }

        Vector2 pos = ConvertScreenPositionToMapPosition(Input.mousePosition);

        if (Input.GetMouseButton(1))
        {
            _map[(int)pos.x, (int)pos.y].SetMaterial(_palette[SelectedMaterial]);
            Color c = _controller.GetColor(_map[(int)pos.x, (int)pos.y]);
            pixels[(int)pos.x * _width + (int)pos.y] = new Vector4(c.r, c.g, c.b, c.a);

            Chunk chunk = GetChunk((int)pos.x, (int)pos.y);
        }

        for (int i = startChunk._x; i <= endChunk._x; i++)
        {
            for (int j = startChunk._y; j <= endChunk._y; j++)
            {
                //GenerateMesh(_chunks[i, j]);
                PaintTexture(_chunks[i, j]);
                _chunks[i, j].HasBeenUpdated = _chunks[i, j].IsUpdated;
                _chunks[i, j].IsUpdated = false;
            }
        }
        GenerateView();
        computeShader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
    }

    private void UpdateMap(Chunk chunk)
    {
        if (chunk.HasBeenUpdated)
        {
            int w = 0;
            int h = 0;
            bool isEven = Time.frameCount % 2 == 0;
            for (int y = 0; y <= chunk.MaxY - chunk.MinY; y++)
            {
                for (int x = 0; x <= chunk.MaxX - chunk.MinX; x++)
                {
                    w = isEven ? chunk.MaxX - x : x + chunk.MinX;
                    h = isEven ? y + chunk.MinY : chunk.MaxY - y;
                    _controller.CalculateMaterialPhysics(_map[w, h]);
                }
            }
        }
    }

    private void GenerateMap()
    {
        _map = new Cell[_width, _height];
        pixels = new Vector4[_width * _height];
        RandomFillMap();
        for (int i = 0; i < 3; i++)
        {
            SmoothMap();
        }

        ProcessMap();
    }

    private void GenerateMesh(Chunk chunk)
    {
        if (chunk.IsUpdated)
        {
            int[,] borderedMap = new int[_width, _height];

            for (int x = chunk.MinX; x <= chunk.MaxX; x++)
            {
                for (int y = chunk.MinY; y <= chunk.MaxY; y++)
                {
                    borderedMap[x, y] = (int)_map[x, y].Material.Type;
                }
            }

            for (int i = 0; i < _chunks.GetLength(0); i++)
            {
                for (int j = 0; j < _chunks.GetLength(1); j++)
                {
                    PaintTexture(_chunks[i, j]);
                    _chunks[i, j].IsUpdated = false;
                }
            }

            MeshGenerator meshGen = GetComponent<MeshGenerator>();
            meshGen.GenerateMesh(borderedMap, 1);
        }
    }

    private void RandomFillMap()
    {
        if (_useRandomSeed)
        {
            _seed = Time.time.ToString();
        }

        System.Random prng = new System.Random(_seed.GetHashCode());

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
                {
                    _map[x, y] = new Cell(x, y, _map, _palette[1], GetChunk(x, y));
                }
                else
                {
                    _map[x, y] = prng.Next(0, 100) < _randomFillPercent ? new Cell(x, y, _map, _palette[1], GetChunk(x, y)) : new Cell(x, y, _map, _palette[0], GetChunk(x, y));
                }
            }
        }
    }


    private void SmoothMap()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                int neighbourWalls = GetSurroundingWallCount(x, y);

                if (neighbourWalls > 4)
                {
                    _map[x, y] = new Cell(x, y, _map, _palette[1], GetChunk(x, y));
                }
                else if (neighbourWalls < 4)
                {
                    _map[x, y] = new Cell(x, y, _map, _palette[0], GetChunk(x, y));
                }
            }
        }
    }

    private int GetSurroundingWallCount(int xGrig, int yGrig)
    {
        int wallCount = 0;

        for (int neighbourX = xGrig - 1; neighbourX <= xGrig + 1; neighbourX++)
        {
            for (int neighbourY = yGrig - 1; neighbourY <= yGrig + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < _width && neighbourY >= 0 && neighbourY < _height)
                {
                    if (neighbourX != xGrig || neighbourY != yGrig)
                    {
                        if (_map[neighbourX, neighbourY].Material.Type == 1)
                        {
                            wallCount++;
                        }
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    private void PaintTexture(Chunk chunk)
    {
        if (chunk.IsUpdated)
        {
            for (int y = chunk.MinY; y <= chunk.MaxY; y++)
            {
                for (int x = chunk.MinX; x <= chunk.MaxX; x++)
                {

                    _map[x, y].UpdateMaterial();

                    if (_map[x, y].HasChanged)
                    {
                        Color c = _controller.GetColor(_map[x, y]);
                        pixels[x * _width + y] = new Vector4(c.r, c.g, c.b, c.a);
                    }
                }
            }
        }
    }

    private void LoadMapSettings()
    {
        if (File.Exists(Application.dataPath + "/Resources/SandboxSettings.json"))
        {
            string data = File.ReadAllText(Application.dataPath + "/Resources/SandboxSettings.json");
            SandboxData loadedData = JsonUtility.FromJson<SandboxData>(data);

            _randomFillPercent = (int)loadedData.fillPercentage;
            _height = int.Parse(loadedData.height);
            _width = int.Parse(loadedData.width);
            _chunkSize = int.Parse(loadedData.chunks);

            if (loadedData.seed == "")
            {
                _seed = "";
                _useRandomSeed = true;
            }
            else
            {
                _seed = loadedData.seed;
            }
        }
    }

    private Chunk GetChunk(int x, int y)
    {
        int w = _width / _chunkSize;
        int chunkx = x / w;
        int h = _height / _chunkSize;
        int chunky = y / h;
        return _chunks[Mathf.Clamp(chunkx, 0, _chunkSize - 1), Mathf.Clamp(chunky, 0, _chunkSize - 1)];
    }

    void ProcessMap()
    {
        List<List<Coord>> roomRegions = GetRegions(0);
        int roomThresholdSize = 500;
        int materialType = 0;

        foreach (List<Coord> roomRegion in roomRegions)
        {
            if (roomRegion.Count < roomThresholdSize)
            {
                var rng = Random.Range(0f, 4f);

                if (rng > 0 && rng < 1)
                {
                    materialType = 2;
                }
                else if (rng >= 1 && rng < 2)
                {
                    materialType = 3;
                }
                else if (rng >= 2 && rng < 3)
                {
                    materialType = 6;
                }
                else
                {
                    materialType = 8;
                }

                foreach (Coord tile in roomRegion)
                {
                    _map[tile.tileX, tile.tileY].SetMaterial(_palette[materialType]);
                }
            }
        }
    }

    List<List<Coord>> GetRegions(int tileType)
    {
        List<List<Coord>> regions = new List<List<Coord>>();
        int[,] mapFlags = new int[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                if (mapFlags[x, y] == 0 && _map[x, y].Material.Type == tileType)
                {
                    List<Coord> newRegion = GetRegionTiles(x, y);
                    regions.Add(newRegion);

                    foreach (Coord tile in newRegion)
                    {
                        mapFlags[tile.tileX, tile.tileY] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Coord> GetRegionTiles(int startX, int startY)
    {
        List<Coord> tiles = new List<Coord>();
        int[,] mapFlags = new int[_width, _height];
        uint tileType = _map[startX, startY].Material.Type;

        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(new Coord(startX, startY));
        mapFlags[startX, startY] = 1;

        while (queue.Count > 0)
        {
            Coord tile = queue.Dequeue();
            tiles.Add(tile);

            for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
            {
                for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                {
                    if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                    {
                        if (mapFlags[x, y] == 0 && _map[x, y].Material.Type == tileType)
                        {
                            mapFlags[x, y] = 1;
                            queue.Enqueue(new Coord(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < _width && y >= 0 && y < _height;
    }

}

public struct Coord
{
    public int tileX;
    public int tileY;

    public Coord(int x, int y)
    {
        tileX = x;
        tileY = y;
    }
}

public struct Render
{
    public Color[,] colorMap;
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
}
