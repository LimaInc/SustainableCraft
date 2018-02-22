﻿using System;
using System.Collections.Generic;
using Godot;

class DefossiliserBlock : CubeBlock
{
    public Defossiliser Machine { private set; get; }

    private static readonly string[] texturePaths = 
        new[] { "res://Images/defossiliser_front.png",
                "res://Images/defossiliser_top.png",
                "res://Images/defossiliser_side.png" };

    public override string[] TexturePaths { get => texturePaths; }

    public override int GetTextureIndex(BlockFace face)
    {
        switch (face)
        {
            case BlockFace.Front:
                return 0;
            case BlockFace.Top:
                return 1;
            default:
                return 2;
        }
    }

    public DefossiliserBlock()
    {
        Machine = new Defossiliser();
    }

    public void HandleInput(InputEvent e)
    {

    }
}
