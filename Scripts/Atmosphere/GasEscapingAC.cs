using System.Collections.Generic;
using System.Linq;
using Godot;

public class GasEscapingAC : AtmosphericComponent
{
    private static readonly float escapability = 0.1f;

    public static float EscapeVelocity { get; } = 10f;

    public override void _PhysicsProcess(float delta)
    {
        atm.GetGases().ForEach(g =>
        {
            float x = GasMolecule.gasData[g].Mass * EscapeVelocity / atm.Temperature;
            // float fractionEscaping = Mathf.Exp(-x);
            float fractionEscaping = escapability / (1 + x);
            float initialAmt = atm.GetGasAmt(g);
            float finalAmt = initialAmt * Mathf.Exp(-fractionEscaping * delta);
            atm.SetGasAmt(g, finalAmt);
        });
    }
}