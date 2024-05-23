using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NetCraft.Models;

public abstract class Model
{
    public required Vector3 Position { get; set; }

    public virtual void Load() { }

    public virtual void Render(Camera cam, Vector3 light) { }
}
