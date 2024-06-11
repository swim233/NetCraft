namespace NetCraft.Models;

public abstract class Capability
{
    public Capability(object @base)
    {
        BaseObject = @base;
    }

    public virtual CapabilityApplyAction? ApplyAction { get; }

    protected object BaseObject;
}

public delegate void CapabilityApplyAction(object item);
