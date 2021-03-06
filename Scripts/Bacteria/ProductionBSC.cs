﻿using Godot;
using System;
using System.Linq;

class ProductionBSC : BacterialStateComponent
{
    protected static Atmosphere atm;

    public override void _Ready()
    {
        atm = GetNode(Game.ATMOSPHERE_PATH) as Atmosphere;
    }

    public override void _PhysicsProcess(float delta)
    {
        delta *= Game.SPEED;
        bs.GetBacteriaList().ForEach(b =>
        {
            atm.ChangeGasAmt(b.GasType, delta * b.Amount * b.ProductionRate);
        });
    }
}
