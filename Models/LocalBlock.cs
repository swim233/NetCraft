using System.Linq.Expressions;
using JeremyAnsel.Media.WavefrontObj;

namespace NetCraft.Models;

// Example model for a cube
public class LocalBlock
{
    private LocalBlock() { }

    private static Dictionary<string, LocalBlock> _cache = [];

    private static ObjFile obj = ObjFile.FromFile("Resources/cube.obj");
    public static float[] Vertices = obj.Vertices.Zip(obj.VertexNormals, (pos, norm) => new { pos, norm }).Zip(obj.TextureVertices, (tuple, tex) => new float[] { tuple.pos.Position.X, tuple.pos.Position.Y, tuple.pos.Position.Z, tuple.norm.X, tuple.norm.Y, tuple.norm.Z, tex.X, tex.Y }).SelectMany(e => e).ToArray();

    public static LocalBlock GetModel(string blockId)
    {
        if (_cache.TryGetValue(blockId, out var value))
        {
            return value;
        }
        else
        {
            LocalBlock model = new();
            _cache.Add(blockId, model);
            return model;
        }
    }
}
