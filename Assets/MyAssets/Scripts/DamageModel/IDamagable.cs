using UnityEngine;

public interface IDamagable
{
    public int health { get; set; }
    public void DoDamage(int damageValue);
}
