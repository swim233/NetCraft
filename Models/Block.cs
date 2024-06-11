using NetCraft.Models.Enums;
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

    public required Vector3i Location { get; init; }
    public Vector2i ChunkLocation => (Location.X % Chunk.SizeX, Location.Z % Chunk.SizeZ);
    public Vector3i LocalLocation => (Location.X - ChunkLocation.X * Chunk.SizeX, Location.Y, Location.Z - ChunkLocation.Y * Chunk.SizeZ);

    public Shader Shader { get; init; }

    public BlockFaceCulling FaceCulling { get; set; } = (BlockFaceCulling)0b111111;

    public List<float> GetVertices(List<float> collection)
    {
        if (FaceCulling.HasFlag(BlockFaceCulling.Top))
            collection.AddRange(BlockModel.Vertices.Take(new Range(0, 6)));
        if (FaceCulling.HasFlag(BlockFaceCulling.Bottom))
            collection.AddRange(BlockModel.Vertices.Take(new Range(6, 12)));
        if (FaceCulling.HasFlag(BlockFaceCulling.XyFront))
            collection.AddRange(BlockModel.Vertices.Take(new Range(12, 18)));
        if (FaceCulling.HasFlag(BlockFaceCulling.XyBack))
            collection.AddRange(BlockModel.Vertices.Take(new Range(18, 24)));
        if (FaceCulling.HasFlag(BlockFaceCulling.ZyFront))
            collection.AddRange(BlockModel.Vertices.Take(new Range(24, 30)));
        if (FaceCulling.HasFlag(BlockFaceCulling.ZyBack))
            collection.AddRange(BlockModel.Vertices.Take(new Range(30, 36)));
        return collection;
    }

    public BlockModel Model { get; init; }

    public void Dump()
    {
        Console.WriteLine($"Block Position(Abs,Chk,Local): {Location} | {ChunkLocation} | {LocalLocation}");
        Console.WriteLine($"Block Face Trimed: {GetFaceCullingString()}");
    }

    protected string GetFaceCullingString()
    {
        System.Text.StringBuilder builder = new();

        if (FaceCulling.HasFlag(BlockFaceCulling.Top))
            builder.Append("Top ");
        if (FaceCulling.HasFlag(BlockFaceCulling.Bottom))
            builder.Append("Bottom ");
        if (FaceCulling.HasFlag(BlockFaceCulling.XyFront))
            builder.Append("XyFront ");
        if (FaceCulling.HasFlag(BlockFaceCulling.XyBack))
            builder.Append("XyBack ");
        if (FaceCulling.HasFlag(BlockFaceCulling.ZyFront))
            builder.Append("ZyFront ");
        if (FaceCulling.HasFlag(BlockFaceCulling.ZyBack))
            builder.Append("ZyBack ");
        return builder.ToString().TrimEnd();
    }

    protected static int CountTrueFlags<T>(T flags)
        where T : Enum
    {
        int count = 0;
        int flagValue = Convert.ToInt32(flags);

        while (flagValue != 0)
        {
            flagValue &= (flagValue - 1);
            count++;
        }

        return count;
    }
}
