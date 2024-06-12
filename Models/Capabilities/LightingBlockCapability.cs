namespace NetCraft.Models.Capabilities;

public sealed class LightingBlockCapability : Capability
{
    public LightingBlockCapability(object @base)
        : base(@base) { }

    public override CapabilityApplyAction ApplyAction =>
        obj =>
        {
            if (obj is not WorldBlock block)
                throw new InvalidOperationException("Not a block.");
            this.block = block;
        };

    private WorldBlock? block;
}
