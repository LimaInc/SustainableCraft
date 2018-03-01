using Godot;

public class IceBlock : CubeBlock
{
    public override string[] TexturePaths { get { return new[] { Game.BLOCK_TEXTURE_PATH + "ice.png" }; } }

    public override int GetTextureIndex(BlockFace face)
    {
        return 0;
    }
}