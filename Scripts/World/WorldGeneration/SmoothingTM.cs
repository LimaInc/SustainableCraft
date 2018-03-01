﻿using Godot;

public class SmoothingTM : ITerrainModifier
{
    private readonly byte STONE_ID = Game.GetBlockId<Stone>();
    private readonly byte RED_ROCK_ID = Game.GetBlockId<RedRock>();
    private const byte AIR_ID = WorldGenerator.AIR_ID;

    private readonly Vector2 center;
    private readonly int centerHeight;

    private readonly float smoothingAtCenter;
    private readonly float halfSmoothingDistance;

    private readonly float sigmoidParamB;
    private readonly float sigmoidParamA;

    public SmoothingTM(Vector2 center, int centerHeight, float smoothingAtCenter, float halfSmoothingDistance)
    {
        this.center = center;
        this.centerHeight = centerHeight;

        this.smoothingAtCenter = smoothingAtCenter;
        this.halfSmoothingDistance = halfSmoothingDistance;

        this.sigmoidParamB = Mathf.Log(this.smoothingAtCenter / (1 - this.smoothingAtCenter));
        this.sigmoidParamA = -this.sigmoidParamB / this.halfSmoothingDistance;
    }

    public void UpdateHeight(Vector2 worldCoords, ref int height)
    {
        // probably overkill to do it for every single place in the world
        // TODO: think up a better solution
        float centerDist = (worldCoords - center).Length();
        float sigmoidSample = MathUtil.Sigmoid(centerDist, sigmoidParamA, sigmoidParamB);
        height = (int)(sigmoidSample * centerHeight + (1 - sigmoidSample) * height);
    }
}