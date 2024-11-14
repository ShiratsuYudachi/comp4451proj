using Godot;
using System;
using System.Collections.Generic;


namespace Chemistry{


public interface IMaterial{
    // public int threshTemperature_High { get; }
    // public int threshTemperature_Low { get; }
    // // Transformative Reactions
    // public void onSwirl(Element element, float elementAmount); // 扩散
    // public void onCrystallize(Element element, float elementAmount); // 结晶
    public void onOverloaded(float elementAmount); // 超载
    public void onElectroCharged(float elementAmount); // 感电
    public void onSuperconduct(float elementAmount); // 超导
    public void onBurning(float elementAmount); // 燃烧

    // public void onBloom(float elementAmount); // 绽放
    // public void onHyperbloom(float elementAmount); // 超绽放  
    // public void onBurgeon(float elementAmount); // 烈绽放
    
    // Amplifying Reactions
    public void onVaporize(float elementAmount); // 蒸发
    public void onMelt(float elementAmount); // 融化
    
    // // Catalyze Reactions  
    // public void onQuicken(float elementAmount); // 激化
    // public void onAggravate(float elementAmount); // 超激化
    // public void onSpread(float elementAmount); // 蔓激化
    
    // Frozen State
    public void onFreeze(float elementAmount); // 冻结

    // public void onReact(Element element1, Element element2);

    // public void onHighTemperature();
    // public void onLowTemperature();
}

public enum Reaction{
    Burning,
    Vaporize,
    Overloaded,
}



public enum Element{
    Pyro, // 火
    Electro, // 雷
    Hydro, // 水
    Dendro,  // 草
    Cryo, // 冰
    Anemo, // 风
    Geo, // 岩
}

// public abstract class Element{
//     public float amount;
//     public Element(float amount){
//         this.amount = amount;
//     }

    
//     public abstract void onAttached(IMaterial material);
    
//     public abstract void onReactWith(Element element);
// }



// public class Pyro : Element{
//     public Pyro(float amount) : base(amount){

//     }

//     public override void onAttached(IMaterial material){
//         material.onBurned();
//     }

//     public override void onReactWith(Element element){
//         if (element is Pyro){
//             this.amount += element.amount;
//         }else if (element is Hydro){
//             this.amount += element.amount;
//         }else if (element is Dendro){
//             this.amount += element.amount;
//         }
//     }
// }

// public class Electro : Element{
//     public Electro(float amount) : base(amount){

//     }
// }

// public class Hydro : Element{
//     public Hydro(float amount) : base(amount){

//     }
// }

// public class Dendro : Element{
//     public Dendro(float amount) : base(amount){

//     }
// }




public class Reactor{
    float Temperature; // °C
    float Acidity;  // pH

    float[] elementAmounts = new float[Enum.GetValues(typeof(Element)).Length];

    IMaterial material;

    public Reactor(){
        this.Temperature = 20;
        this.Acidity = 7;
    }

    public void SetMaterial(IMaterial material){
        this.material = material;
    }


    public void AddElement(Element elementType, float amount){
        this.elementAmounts[(int)elementType] += amount;
    }


    private bool _haveElement(Element elementType){
        return this.elementAmounts[(int)elementType] > 0;
    }

    public Element[] GetActiveElements(){
        List<Element> activeElements = new List<Element>();
        foreach (Element element in Enum.GetValues(typeof(Element))){
            if (_haveElement(element)){
                activeElements.Add(element);
            }
        }
        return activeElements.ToArray();
    }

    public float GetElementAmount(Element elementType){
        return this.elementAmounts[(int)elementType];
    }

    // private void _assimilate(Element elementToIncrease, Element elementToDecrease){ 
    //     this.elementAmounts[(int)elementToIncrease] += this.elementAmounts[(int)elementToDecrease];
    //     this.elementAmounts[(int)elementToDecrease] = 0;
    // }

    // private void _alienate(Element elementSrc, Element elementDst){
    //     this.elementAmounts[(int)elementSrc] -= this.elementAmounts[(int)elementDst];
    //     this.elementAmounts[(int)elementDst] = 0;
    // }


    private float _consumeMin(Element element1, Element element2){
        float amountConsumed = Mathf.Min(this.elementAmounts[(int)element1], this.elementAmounts[(int)element2]);
        this.elementAmounts[(int)element1] -= amountConsumed;
        this.elementAmounts[(int)element2] -= amountConsumed;
        return amountConsumed;
    }



    public void Update(double delta){
        // Burning (Pyro + Dendro)
        if (_haveElement(Element.Pyro) && _haveElement(Element.Dendro)){
            material.onBurning(_consumeMin(Element.Pyro, Element.Dendro));
        }
        
        // Vaporize (Pyro + Hydro)
        if (_haveElement(Element.Pyro) && _haveElement(Element.Hydro)){
            material.onVaporize(_consumeMin(Element.Pyro, Element.Hydro));
        }
        
        // Overload (Pyro + Electro)
        if (_haveElement(Element.Pyro) && _haveElement(Element.Electro)){
            material.onOverloaded(_consumeMin(Element.Pyro, Element.Electro));
        }
        
        // Melt (Pyro + Cryo)
        if (_haveElement(Element.Pyro) && _haveElement(Element.Cryo)){
            material.onMelt(_consumeMin(Element.Pyro, Element.Cryo));
        }
        
        // // Swirl (Pyro + Anemo)
        // if (_haveElement(Element.Pyro) && _haveElement(Element.Anemo)){
        //     material.onSwirl(Element.Pyro, _consumeMin(Element.Pyro, Element.Anemo));
        // }
        
        // // Crystallize (Pyro + Geo)
        // if (_haveElement(Element.Pyro) && _haveElement(Element.Geo)){
        //     material.onCrystallize(Element.Pyro, _consumeMin(Element.Pyro, Element.Geo));
        // }
        
        // === Cryo Reactions ===
        // Superconduct (Cryo + Electro)
        if (_haveElement(Element.Cryo) && _haveElement(Element.Electro)){
            material.onSuperconduct(_consumeMin(Element.Cryo, Element.Electro));
        }
        
        // Freeze (Cryo + Hydro)
        if (_haveElement(Element.Cryo) && _haveElement(Element.Hydro)){
            material.onFreeze(_consumeMin(Element.Cryo, Element.Hydro));
        }
        
        // // Swirl (Cryo + Anemo)
        // if (_haveElement(Element.Cryo) && _haveElement(Element.Anemo)){
        //     material.onSwirl(Element.Cryo, _consumeMin(Element.Cryo, Element.Anemo));
        // }
        
        // // Crystallize (Cryo + Geo)
        // if (_haveElement(Element.Cryo) && _haveElement(Element.Geo)){
        //     material.onCrystallize(Element.Cryo, _consumeMin(Element.Cryo, Element.Geo));
        // }

        // === Hydro Reactions ===
        // Electro-Charged (Hydro + Electro)
        if (_haveElement(Element.Hydro) && _haveElement(Element.Electro)){
            material.onElectroCharged(_consumeMin(Element.Hydro, Element.Electro));
        }
        
        // // Bloom (Hydro + Dendro)
        // if (_haveElement(Element.Hydro) && _haveElement(Element.Dendro)){
        //     material.onBloom(_consumeMin(Element.Hydro, Element.Dendro));
        // }
        
        // // Swirl (Hydro + Anemo)
        // if (_haveElement(Element.Hydro) && _haveElement(Element.Anemo)){
        //     material.onSwirl(Element.Hydro, _consumeMin(Element.Hydro, Element.Anemo));
        // }
        
        // // Crystallize (Hydro + Geo)
        // if (_haveElement(Element.Hydro) && _haveElement(Element.Geo)){
        //     material.onCrystallize(Element.Hydro, _consumeMin(Element.Hydro, Element.Geo));
        // }


        // Decay all elements by 1 per second
        foreach (Element element in Enum.GetValues(typeof(Element))){
            if (this.elementAmounts[(int)element] > 0){
                this.elementAmounts[(int)element] -= (float)delta;
            }
        }
        
        
        
    }

    public void PrintElementAmounts(){
        foreach (Element element in Enum.GetValues(typeof(Element))){
            GD.Print(element.ToString() + ": " + this.elementAmounts[(int)element]);
        }
    }



}

}