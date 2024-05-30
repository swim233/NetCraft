using OpenTK.Mathematics;

namespace NetCraft;

public class Chunk
{
    public Block?[,,] Blocks { get; set; } = new Block[SizeX, SizeY, SizeZ];

    public Vector2i Location { get; init; }

    public const int SizeX = 16;
    public const int GenerateSizeX = 16;
    public const int SizeY = 256;
    public const int GenerateSizeY = 16;
    public const int SizeZ = 16;
    public const int GenerateSizeZ = 16;

    private Stopwatch watch = new();

    public Chunk(Vector2i location)
    {
        Location = location;

        watch.Start();
        for (int x = 0; x < GenerateSizeX; x++)
        for (int y = 0; y < GenerateSizeY; y++)
        for (int z = 0; z < GenerateSizeZ; z++)
        {
            Blocks[x, y, z] = new Block("container2") { Position = new(x + Location.X * SizeX, y, z + Location.Y * SizeZ), };
        }
        Console.WriteLine("Construct time(ms): " + watch.Elapsed.TotalMilliseconds);
        watch.Reset();
    }

    public void Load()
    {
        watch.Start();
        for (int x = 0; x < SizeX; x++)
        for (int y = 0; y < SizeY; y++)
        for (int z = 0; z < SizeZ; z++)
        {
            Blocks[x, y, z]?.Load();
        }
        Console.WriteLine("Load time(ms): " + watch.Elapsed.TotalMilliseconds);
        watch.Restart();

        for (int x = 0; x < GenerateSizeX; x++)
        for (int y = 0; y < GenerateSizeY; y++)
        for (int z = 0; z < GenerateSizeZ; z++)
        {
            Block? block = Blocks[x, y, z];
            if (block is null)
                continue;
            if (y < SizeY - 1 && Blocks[x, y + 1, z] is not null)
                block.DrawTop = false;
            if (y > 0 && Blocks[x, y - 1, z] is not null)
                block.DrawBottom = false;
            if (x < SizeX - 1 && Blocks[x + 1, y, z] is not null)
                block.DrawYzFront = false;
            if (x > 0 && Blocks[x - 1, y, z] is not null)
                block.DrawYzBack = false;
            if (z < SizeZ - 1 && Blocks[x, y, z + 1] is not null)
                block.DrawXyFront = false;
            if (z > 0 && Blocks[x, y, z - 1] is not null)
                block.DrawXyBack = false;
        }
        Console.WriteLine("Facecull time(ms): " + watch.Elapsed.TotalMilliseconds);
        watch.Reset();
    }

    public void Render(Camera camera, Vector3 light)
    {
        int count = 0;
        for (int x = 0; x < SizeX; x++)
        for (int y = 0; y < SizeY; y++)
        for (int z = 0; z < SizeZ; z++)
        {
            Blocks[x, y, z]?.Render(camera, light);
            count++;
        }
    }
}
