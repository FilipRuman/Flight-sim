
using Godot;

public partial class TerrainGeneration : Node3D
{
    [Export] ShaderMaterial material;
    [Export] MeshInstance3D mesh;
    [Export] CollisionShape3D collisionShape;
    public float size = 200;
    [Export] int resolution = 2;
    public int seed = 0;
    public float heightModifier = 0;


    public float waterLevelHeight;
    [Export] public MeshInstance3D water;



    public FastNoiseLite noise;
    public FastNoiseLite noise2;

    private float GetHeight(Vector2 pos)
    {
        float value = noise.GetNoise2D(pos.X + GlobalPosition.X, pos.Y + GlobalPosition.Z);
        value += noise2.GetNoise2D(pos.X + GlobalPosition.X, pos.Y + GlobalPosition.Z);

        return value * heightModifier;
    }

    private Vector3 GetNormal(Vector2 pos)
    {
        var epsilon = size / resolution;

        var x = GetHeight(new(pos.X + epsilon, pos.Y)) - GetHeight(new(pos.X - epsilon, pos.Y)) / (2 * epsilon);
        var y = 1;
        var z = GetHeight(new(pos.X, pos.Y + epsilon)) - GetHeight(new(pos.X, pos.Y - epsilon)) / (2 * epsilon);

        return new Vector3(x, y, z).Normalized();
    }
    public void UpdateMesh()
    {
        var all = System.Diagnostics.Stopwatch.StartNew();


        var arrayMesh = new ArrayMesh();



        var plane = new PlaneMesh
        {
            SubdivideDepth = resolution,
            SubdivideWidth = resolution,
            Size = Vector2.One * size
        };
        var waterMesh = new PlaneMesh
        {
            SubdivideDepth = resolution / 2,
            SubdivideWidth = resolution / 2,
            Size = Vector2.One * size
        };
        water.Mesh = waterMesh;
        water.Position += Vector3.Up * waterLevelHeight;


        Godot.Collections.Array planeArrays = plane.GetMeshArrays();
        var vertexArray = planeArrays[(int)Mesh.ArrayType.Vertex].As<Vector3[]>();
        var normalArray = planeArrays[(int)Mesh.ArrayType.Normal].As<Vector3[]>();
        var tangentArray = planeArrays[(int)Mesh.ArrayType.Tangent].As<float[]>();



        var noise = System.Diagnostics.Stopwatch.StartNew();

        for (int i = 0; i < vertexArray.Length; i++)
        {
            var vertex = vertexArray[i];


            Vector2 noisePosition = new(vertex.X, vertex.Z);
            vertex.Y = GetHeight(noisePosition);
            var normal = GetNormal(noisePosition);
            var tangent = normal.Cross(Vector3.Up);
            vertexArray[i] = vertex;
            normalArray[i] = normal;
            tangentArray[4 * i] = tangent.X;
            tangentArray[4 * i + 1] = tangent.Y;
            tangentArray[4 * i + 2] = tangent.Z;

        }

        GD.Print($"Time- noise: {noise.Elapsed}");

        noise.Stop();
        planeArrays[(int)Mesh.ArrayType.Vertex] = vertexArray;
        planeArrays[(int)Mesh.ArrayType.Normal] = normalArray;
        planeArrays[(int)Mesh.ArrayType.Tangent] = tangentArray;





        arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, planeArrays);
        mesh.Mesh = arrayMesh;
        collisionShape.Shape = arrayMesh.CreateTrimeshShape();
        mesh.SetSurfaceOverrideMaterial(0, material);
        GD.Print($"Time- all: {all.Elapsed}");

        all.Stop();
    }

}
