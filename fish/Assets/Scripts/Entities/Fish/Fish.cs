using UnityEngine;

public class Fish
{
    public string name;
    public float pullForce, pushForce;
    public float jumpCD;
    public float healAmount;
    public int width, height;

    public Fish(string name, float pullForce, float pushForce, float jumpCD, float healAmount, int width, int height)
    {
        this.name = name;
        this.pullForce = pullForce;
        this.pushForce = pushForce;
        this.jumpCD = jumpCD;
        this.healAmount = healAmount;
        this.width = width;
        this.height = height;
    }
}
