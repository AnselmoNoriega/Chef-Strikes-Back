using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public abstract string Name { get; }
    public abstract int Damage { get; }
    public abstract float Range { get; }
}


public class Knife : Weapon
{
    public override string Name => "Knife";
    public override int Damage => 10;
    public override float Range => 1.0f;
}

public class Colander : Weapon
{
    public override string Name => "Colander";
    public override int Damage => 15;
    public override float Range => 1.2f;
}

public class Spatula : Weapon
{
    public override string Name => "Spatula";
    public override int Damage => 20;
    public override float Range => 1.5f;
}
