using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace NetCraft.Models;

public class Block
{
    public Block(string blockId)
    {
        Shader = Shader.GetShaderFromId(_blockMap.Contains(blockId) ? blockId : "blockNormal");
        Model = BlockModel.GetModel(blockId);
    }

    private static List<string> _blockMap = new() { "blockLamp" };

    public required Vector3i Position { get; init; }
    public required Vector2i ChunkLocation { get; init; }
    public Vector3i LocalPosition => (Position.X % (ChunkLocation.X * Chunk.SizeX), Position.Y, Position.Z % (ChunkLocation.Y * Chunk.SizeZ));

    public Shader Shader { get; init; }

    public bool DrawTop { get; set; } = true;
    public bool DrawBottom { get; set; } = true;
    public bool DrawXyFront { get; set; } = true;
    public bool DrawXyBack { get; set; } = true;
    public bool DrawYzFront { get; set; } = true;
    public bool DrawYzBack { get; set; } = true;

    public BlockModel Model { get; init; }

    public void Dump()
    {
        Console.WriteLine($"Block Position(Abs,Chk,Local): {Position} | {ChunkLocation} | {LocalPosition}");
        Console.WriteLine($"Block Render: Top({DrawTop}) Bottom({DrawBottom}) XyFront({DrawXyFront}) XyBack({DrawXyBack}) YzFront({DrawYzFront}) YzBack({DrawYzBack})");
    }
}
