namespace NetCraft.Models.Capabilities;

public sealed class LightingBlockCapability : Capability
{
    public LightingBlockCapability(object @base)
        : base(@base) { }

    public override CapabilityApplyAction ApplyAction =>
        obj =>
        {
            if (obj is not Block block)
                throw new InvalidOperationException("Not a block.");
            this.block = block;
        };

    private Block? block;
}
