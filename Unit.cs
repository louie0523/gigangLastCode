using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public float Maxhp;
    public float Hp;
    public float Speed;
    public float Defend;

    public bool isAlive = true;
    public bool isKnockBack = false;

    public Rigidbody rb;
    public Material Material;
    public Color OriginColor;


    protected virtual void Start()
    {
        Hp = Maxhp;


    }

    public virtual void Damage(float damage)
    {
        if (!isAlive)
            return;

        damage *= 1 - (Defend * 0.01f);

        Hp -= damage;

        if(Hp <= 0)
        {
            isAlive = false;
            Death();
        }
    }

    public virtual void Heal(float heal)
    {
        if (!isAlive) return;

        if(Hp > Maxhp)
        {
            Hp = Maxhp;
        }
    }

    public virtual void Death()
    {

    }

    public IEnumerator KnockBack(float power, float time)
    {
        if (isKnockBack)
            yield return null;

        isKnockBack = true;
        rb.velocity = -transform.forward * power;
        yield return new WaitForSeconds(time);
        isKnockBack = false;
    }

}
