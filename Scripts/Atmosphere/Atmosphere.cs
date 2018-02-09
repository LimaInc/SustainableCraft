using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Atmosphere : Node
{
    public enum Gas
    {
        OXYGEN, NITROGEN, CARBON_DIOXIDE,
        VACUUM
    }

    private static readonly IDictionary<Gas, float> gasGoals = new Dictionary<Gas, float>
    {
        [Gas.OXYGEN] = 2,
        [Gas.NITROGEN] = 3,
        [Gas.CARBON_DIOXIDE] = 1
    };
    private IDictionary<Gas,float> gases;
    private IAtmosphericComponent dynamics;
    private IAtmosphericComponent graphics;

    public Atmosphere(IAtmosphericComponent dynamics, IAtmosphericComponent graphics)
    {
        gases = new Dictionary<Gas,float>();
        foreach (Gas g in Enum.GetValues(typeof(Gas)))
        {
            gases[g] = 0;
        }
        this.dynamics = dynamics;
        this.graphics = graphics;
    }

    public Double GetGasAmt(Gas gas) => gases[gas];

    public IList<Gas> GetGases() => gases.Keys.ToList();

    public void SetGas(Gas gas, float amt)
    {
        gases[gas] = amt;
    }

    public float GetGasProgress(Gas gas)
    {
        return Math.Min(gases[gas]/gasGoals[gas],1);
    }

    public void Update(float delta, ExoWorld world)
    {
        dynamics.Update(delta, world, this);
        graphics.Update(delta, world, this);
    }
}