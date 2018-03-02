using System;
using Godot;

public class HabitationBlock : CubeBlock
{
    public override bool Breakable { get { return false; } }
    public override string[] TexturePaths { get { return new[] { Game.BLOCK_TEXTURE_PATH + "habitationBlock.png" }; } }

    public override int GetTextureIndex(BlockFace face)
    {
        return 0;
    }
}