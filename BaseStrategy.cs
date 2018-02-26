using Godot;
using System;
using System.Collections.Generic;

public abstract class BaseStrategy : Node
{
    public BaseStrategy(AnimalBehaviourComponent component)
    {
        this.component = component;
    }

    protected AnimalBehaviourComponent component;

    public abstract void Ready();
    public abstract void StartState(params object[] args);
    public abstract void PhysicsProcess(float delta);
    public abstract void Process(float delta);

    public virtual void Collided(KinematicCollision collision) { }
    public virtual void ObjectInRange(PhysicsBody body) { }
    public virtual void ObjectOutOfRange(PhysicsBody body) { }

    public virtual List<Tuple<string, Node, string>> GetConnections() { return new List<Tuple<string, Node, string>>(); }

    public virtual List<Tuple<string, object[]>> GetInitialisationSignals() { return new List<Tuple<string, object[]>>(); }

    public bool active
    {
        get { return active; }
        set
        {
            if (value)
            {
                component._SetActiveState(this);
            }
            else
            {
                component._DeactivateState(this);
            }
            active = value;
        }
    }
}