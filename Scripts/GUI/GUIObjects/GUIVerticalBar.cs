using Godot;
using System;

public class GUIVerticalBar : GUIObject
{
    public const string BAR = "bar";
    public const float WIDTH = 64;

    private ColorRect cRect;
    protected float height;
    private float perc;
    public float Percentage
    {
        get => perc;
        set {
            float newValue = Mathf.Clamp(value, 0, 1);
            if (perc != newValue)
            {
                perc = newValue;
                cRect.SetPosition(rect.Position + new Vector2(0, (1 - perc) * height));
                cRect.SetSize(new Vector2(WIDTH, perc * height));
            }
        }
    }

    private Color color;
    private Func<float> percentageSupplier;

    public virtual Vector2 Size { get => new Vector2(WIDTH, height); }
    
    public GUIVerticalBar(Vector2 pos, float height_, Color color_, Func<float> percentageSupplier_ = null, Func<bool> shouldShow = null)
        : base(pos, new Vector2(WIDTH,height_), Game.guiResourceLoader.GetResource(BAR) as Texture, shouldShow)
    {   
        height = height_;
        color = color_;
        percentageSupplier = percentageSupplier_;

        cRect = new ColorRect
        {
            Color = color
        };

        RemoveChild(sprite);
        AddChild(cRect);
        AddChild(sprite);
    }

    public override void _Process(float delta)
    {
        if (percentageSupplier != null)
        {
            Percentage = percentageSupplier();
        }
    }
}