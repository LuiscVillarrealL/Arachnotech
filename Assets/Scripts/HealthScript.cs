using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript : MonoBehaviour
{

    [SerializeField]
    private int maxHp = 100;
    public int hp;

    public int MaxHp => maxHp;
    public int Hp
    {
        get => hp;
        private set {

            var isDamaged = value < hp;
            hp = Mathf.Clamp(value, 0, maxHp);
            Debug.Log("dentro private");
            if (isDamaged)
            {
                Damaged?.Invoke(hp);
            }
            else
            {
                Healed?.Invoke(hp);
            }

            if (hp <= 0)
            {
                Died?.Invoke();
            }
        }
    }

    public UnityEvent<int> Damaged;
    public UnityEvent<int> Healed;
   
    public UnityEvent Died;

    private void Awake()
    {
        hp = maxHp;
    }

    public void Damage(int damage) => Hp -= damage;

    public void Heal(int heal) => Hp += heal;

    public void FullHeal() => Hp = maxHp;

    public void Kill() => Hp = 0;

    public void AdjustHp(int hpAdjust) => Hp = hpAdjust;


    
}
