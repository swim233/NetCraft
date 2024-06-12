using NetCraft.Models.Lights;

namespace NetCraft.Models;

public class BlockPointLight : WorldBlock, IPointLight
{
    public BlockPointLight(string blockId)
        : base(blockId) { }

    public required PointLight PointLight { get; init; }
}
