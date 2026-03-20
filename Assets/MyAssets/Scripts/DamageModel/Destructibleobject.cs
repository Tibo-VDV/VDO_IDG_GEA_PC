using UnityEngine;

public class Destructibleobject : MonoBehaviour, IDamagable
{
    [SerializeField] int _health;
    public int health { get { return _health; } set { _health = value; } }// variable health en _health gaan altijd dezelfde value hebben
    [SerializeField] GameObject DestroyedState;

    public void DoDamage(int damageValue)
    {
        health-= damageValue;
        if (health <= 0)
        {
            OnKill();
        }
    }
    
    void OnKill()
    {
        Instantiate(DestroyedState, transform.position, Quaternion.identity);
        Destroy(gameObject);//Destroyes obj waar dit script op staat niet net gespawned object
        
    }



}
