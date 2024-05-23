using OpenTK.Mathematics;

namespace NetCraft;

public class Chunk
{
    public Block[,,] Blocks { get; set; } = new Block[SizeX, SizeY, SizeZ];

    public (int, int) Location { get; init; }

    public const int SizeX = 16;
    public const int SizeY = 16;
    public const int SizeZ = 16;

    private Stopwatch watch = new();

    public Chunk(Shader shader, (int, int) location)
    {
        Location = location;

        watch.Start();
        for(int x = 0; x < SizeX; x++)
        for(int y = 0; y < SizeY; y++)
        for(int z = 0; z < SizeZ; z++)
        {
            Blocks[x, y, z] = new Block()
                {
                    DiffuseMapPath = "Resources/container2.png",
                    SpecularMapPath = "Resources/container2_specular.png",
                    Shader = shader,
                    Position = new (x + Location.Item1 * SizeX, y, z + Location.Item2 * SizeZ),
                    HasNormal = true,
                    HasTexture = true,
                };
        }
        Console.WriteLine("Construct time(ms): " + watch.Elapsed.TotalMilliseconds);
        watch.Restart();

        for(int x = 0; x < SizeX; x++)
        for(int y = 0; y < SizeY; y++)
        for(int z = 0; z < SizeZ; z++)
        {
            Block? block = Blocks[x,y,z];
            if(block is null)
                continue;
            if(y < SizeY-1 && Blocks[x,y+1,z] is not null)
                block.DrawTop = false;
            if(y > 0 && Blocks[x,y-1,z] is not null)
                block.DrawBottom = false;
            if(x < SizeX-1 && Blocks[x+1,y,z] is not null)
                block.DrawYzFront = false;
            if(x > 0 && Blocks[x-1,y,z] is not null)
                block.DrawYzBack = false;
            if(z < SizeZ-1 && Blocks[x,y,z+1] is not null)
                block.DrawXyFront = false;
            if(z > 0 && Blocks[x,y,z-1] is not null)
                block.DrawXyBack = false;
        }
        Console.WriteLine("Facecull time(ms): " + watch.Elapsed.TotalMilliseconds);
        watch.Reset();
    }

    public void Load()
    {
        watch.Start();
        for(int x = 0; x < SizeX; x++)
        for(int y = 0; y < SizeY; y++)
        for(int z = 0; z < SizeZ; z++)
        {
            Blocks[x,y,z]?.Load();
        }
        Console.WriteLine("Load time(ms): " + watch.Elapsed.TotalMilliseconds);
        watch.Reset();
    }

    public void Render(Camera camera, Vector3 light)
    {
        int count = 0;
        for(int x = 0; x < SizeX; x++)
        for(int y = 0; y < SizeY; y++)
        for(int z = 0; z < SizeZ; z++)
        {
            Blocks[x, y, z].Render(camera, light);
            count++;
        }
    }
}
