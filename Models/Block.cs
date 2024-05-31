using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NetCraft.Models;

public class Block
{
    public Block(string blockId)
    {
        _shader = Shader.GetShaderFromId(_blockMap.Contains(blockId) ? blockId : "blockNormal");
        Model = BlockModel.GetModel(blockId);
    }

    private static List<string> _blockMap = new() { "blockLamp" };

    private int _vertexBufferObject;
    private int _vertexArrayObject;

    public Vector3i Position { get; set; }

    private Shader _shader = null!;

    public bool DrawTop { get; set; } = true;
    public bool DrawBottom { get; set; } = true;
    public bool DrawXyFront { get; set; } = true;
    public bool DrawXyBack { get; set; } = true;
    public bool DrawYzFront { get; set; } = true;
    public bool DrawYzBack { get; set; } = true;

    public BlockModel Model { get; init; }

    public void Load()
    {
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, Model.Vertices.Length * sizeof(float), Model.Vertices, BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        var positionLocation = _shader.GetAttribLocation("aPos");
        GL.EnableVertexAttribArray(positionLocation);
        GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

        var normalLocation = _shader.GetAttribLocation("aNormal");
        GL.EnableVertexAttribArray(normalLocation);
        GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

        if (Model.DiffuseMap is not null)
        {
            var texCoordLocation = _shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
        }
    }

    public void Render(Camera camera, Vector3 light)
    {
        GL.BindVertexArray(_vertexArrayObject);

        Model.DiffuseMap?.Use(TextureUnit.Texture0);
        Model.SpecularMap?.Use(TextureUnit.Texture1);
        _shader.Use();

        _shader.SetMatrix4("model", Matrix4.Identity * Matrix4.CreateTranslation(Position));
        _shader.SetMatrix4("view", camera.GetViewMatrix());
        _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

        if (Model.DiffuseMap is not null)
        {
            _shader.SetVector3("viewPos", camera.Position);
            _shader.SetInt("material.diffuse", 0);
        }
        if (Model.SpecularMap is not null)
        {
            _shader.SetInt("material.specular", 1);
            _shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _shader.SetFloat("material.shininess", 32.0f);

            _shader.SetVector3("light.position", light);
            _shader.SetVector3("light.ambient", new Vector3(0.2f));
            _shader.SetVector3("light.diffuse", new Vector3(0.5f));
            _shader.SetVector3("light.specular", new Vector3(1.0f));
        }

        if (DrawXyBack)
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        if (DrawXyFront)
            GL.DrawArrays(PrimitiveType.Triangles, 6, 6);
        if (DrawYzBack)
            GL.DrawArrays(PrimitiveType.Triangles, 12, 6);
        if (DrawYzFront)
            GL.DrawArrays(PrimitiveType.Triangles, 18, 6);
        if (DrawBottom)
            GL.DrawArrays(PrimitiveType.Triangles, 24, 6);
        if (DrawTop)
            GL.DrawArrays(PrimitiveType.Triangles, 30, 6);
    }

    public void Dump()
    {
        Console.WriteLine("Block Position: " + Position);
        Console.WriteLine($"Block Render: Top({DrawTop}) Bottom({DrawBottom}) XyFront({DrawXyFront}) XyBack({DrawXyBack}) YzFront({DrawYzFront}) YzBack({DrawYzBack})");
    }
}
