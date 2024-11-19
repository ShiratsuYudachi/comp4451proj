using System;
using Godot;
using Chemistry;

public partial class ReactionTipLabel : TipLabel
{
    public Reaction reaction;

    public override void _Ready()
    {
        base._Ready();
        Text = reaction.ToString();
        switch (reaction)
        {
            case Reaction.Burning:
                LabelSettings.OutlineColor = Colors.Red;
                break;
            case Reaction.Vaporize:
                LabelSettings.OutlineColor = Colors.Blue;
                break;
            case Reaction.Superconduct:
                LabelSettings.OutlineColor = Colors.Purple;
                break;
            case Reaction.Freeze:
                LabelSettings.OutlineColor = Colors.LightBlue;
                break;
            case Reaction.Overloaded:
                LabelSettings.OutlineColor = Colors.Orange;
                break;
        }

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
