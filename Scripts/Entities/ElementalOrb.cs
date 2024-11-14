using Godot;
using System;
using Chemistry;

public partial class ElementalOrb : Bullet
{
	Element element;


    protected override void onHitEntity(Entity entity){
        entity.reactor.AddElement(element, 10); 
		base.onHitEntity(entity);
	}

    public void SetElement(Element element){
        this.element = element;
        switch (element){
            case Element.Pyro:
                this.Modulate = Colors.Red;
                break;
            case Element.Hydro:
                this.Modulate = Colors.Blue;
                break;
            case Element.Electro:
                this.Modulate = Colors.Purple;
                break;  
            case Element.Cryo:
                this.Modulate = Colors.LightBlue;
                break;
            case Element.Geo:
                this.Modulate = Colors.Yellow;
                break;
            case Element.Anemo:
                this.Modulate = Colors.LightGreen;
                break;
            case Element.Dendro:
                this.Modulate = Colors.Green;
                break;
        }
    }

}
