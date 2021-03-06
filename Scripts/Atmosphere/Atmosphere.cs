using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Atmosphere : Node
{
    private const float DEFAULT_TEMPERATURE = 300;

    private static readonly IDictionary<Gas, float> gasGoals = new Dictionary<Gas, float>
    {
        [Gas.OXYGEN] = 0.2f,
        [Gas.NITROGEN] = 0.7f,
        [Gas.CARBON_DIOXIDE] = 0.1f
    };
    private IDictionary<Gas,float> gases;

    public float Pressure { get; private set; }
    public float Temperature { get; private set; }

    public Atmosphere()
    {
        Temperature = DEFAULT_TEMPERATURE;
        gases = new Dictionary<Gas, float>();
        foreach (Gas g in Enum.GetValues(typeof(Gas)))
        {
            gases[g] = 0;
        }
    }

    public void AddComponents(params AtmosphericComponent[] components)
    {
        foreach(AtmosphericComponent cmp in components)
        {
            this.AddChild(cmp);
        }
    }

    public float GetGasAmt(Gas gas) => gases[gas];

    public List<Gas> GetGases() => gases.Keys.ToList();

    public void SetGasAmt(Gas gas, float amt)
    {
        gases[gas] = amt;
    }

    public void ChangeGasAmt(Gas gas, float amt)
    {
        gases[gas] += amt;
    }

    public float GetGasProgress(Gas gas)
    {
        return Mathf.Clamp(gases[gas]/gasGoals[gas],0,1);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        ValidateState();
    }

    private void ValidateState()
    {
        UpdatePressure();
    }

    private void UpdatePressure()
    {
        Pressure = gases.Values.Sum();
    }
}