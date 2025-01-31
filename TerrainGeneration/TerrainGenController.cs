using Godot;
using System;
using System.Collections.Generic;
[Tool]
public partial class TerrainGenController : Node3D
{
    [Export] bool forceRegenerate = new();
    [Export] PackedScene TerrainGenerationPrefab;
    [Export] Node3D terrainLayout;
    [Export] Node3D Player;
    [Export] FastNoiseLite noise;
    [Export] FastNoiseLite noise2;
    [Export] float ViewDistance;
    [Export] float terrainSize;
    Dictionary<Vector2I, TerrainGeneration> terrainDisplays = new();
    [Export] int seed;
    [Export] float heightModifier;
    [Export] int framesPerUpdate = 100;
    [Export] float waterLevelHeight;
    int frameIndex = 0;



    int lastMaxX, lastMinX, lastMaxY, lastMinY;



    bool LoadNewTerrain(int maxX, int minX, int maxY, int minY)
    {
        var returnValue = !(lastMaxX == maxX && lastMinX == minX && lastMinY == minY && lastMaxY == maxY);
        if (returnValue)
        {
            lastMaxX = maxX;
            lastMinX = minX;
            lastMinY = minY;
            lastMaxY = maxY;
        }
        return returnValue;
    }
    public override void _Process(double delta)
    {
        frameIndex++;
        if (frameIndex < framesPerUpdate) return;
        frameIndex = 0;
        if (terrainLayout == null) return;
        noise.Seed = seed;
        noise2.Seed = seed;
        WhatTerrainDoYouNeedToLoad(out int maxX, out int minX, out int maxY, out int minY);

        if (!LoadNewTerrain(maxX, minX, maxY, minY) && !forceRegenerate) return;
        if (forceRegenerate)
            Regenerate();

        forceRegenerate = false;

        RemoveNotNeededTerrain(maxX, minX, maxY, minY);
        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                Vector2I key = new(x, y);
                if (terrainDisplays.ContainsKey(key))
                    continue;
                SpawnTerrain(key);
            }
        }


        base._Process(delta);
    }

    private void Regenerate()
    {
        foreach (var item in terrainLayout.GetChildren())
        {
            item.QueueFree();
        }
        terrainDisplays.Clear();
    }


    void RemoveNotNeededTerrain(int maxX, int minX, int maxY, int minY)
    {
        List<Vector2I> toRemove = new();
        foreach (var terrainValePair in terrainDisplays)
        {
            if (terrainValePair.Key.X < minX || terrainValePair.Key.X > maxX ||
                terrainValePair.Key.Y < minY || terrainValePair.Key.Y > maxY)
            {
                terrainValePair.Value.QueueFree();
                toRemove.Add(terrainValePair.Key);
            }

        }

        foreach (var item in toRemove)
        {
            terrainDisplays.Remove(item);
        }
    }

    void WhatTerrainDoYouNeedToLoad(out int maxX, out int minX, out int maxY, out int minY)
    {
        var playerPos = Player.GlobalPosition;

        maxX = Mathf.CeilToInt((playerPos.X + ViewDistance) / terrainSize);
        minX = Mathf.CeilToInt((playerPos.X - ViewDistance) / terrainSize);

        maxY = Mathf.CeilToInt((playerPos.Z + ViewDistance) / terrainSize);
        minY = Mathf.CeilToInt((playerPos.Z - ViewDistance) / terrainSize);
    }

    void SpawnTerrain(Vector2I positionOnAGrid)
    {
        var node = TerrainGenerationPrefab.Instantiate(PackedScene.GenEditState.Main);
        terrainLayout.AddChild(node);
        var terrain = (TerrainGeneration)node;

        terrainDisplays.Add(positionOnAGrid, terrain);

        terrain.size = terrainSize;
        terrain.seed = seed;
        terrain.waterLevelHeight = waterLevelHeight;

        terrain.heightModifier = heightModifier;

        Vector3 position = new(positionOnAGrid.X * terrainSize, 0, positionOnAGrid.Y * terrainSize);
        terrain.Position = position;

        terrain.noise = noise;
        terrain.noise2 = noise2;
        var updMesh = System.Diagnostics.Stopwatch.StartNew();

        terrain.UpdateMesh();
        GD.Print($"Time- updMesh: {updMesh.Elapsed}");
        updMesh.Stop();

    }


}
