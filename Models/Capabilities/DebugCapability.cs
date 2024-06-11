public sealed class DebugCapability : Capability
{
    public DebugCapability(object @base)
        : base(@base) { }

    public void Dump()
    {
        switch (BaseObject)
        {
            case var obj when obj is Block block:
                Console.WriteLine($"Block Position(Abs,Chk,Local): {block.Location} | {block.ChunkLocation} | {block.LocalLocation}");
                Console.WriteLine($"Block Face Trimed: {block.GetFaceCullingString()}");
                break;
            case null:
                Console.WriteLine($"Dumping null");
                break;
            default:
                Console.WriteLine($"Dumping item {BaseObject}");
                break;
        }
    }
}
