using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Type;
    public Transform Target;
    public Vector3 Targetpos;
    public float Speed;
    public float Damage;
    public float Lifetime;
    public float currTime;
    public float arkhegiht = 4f;

    public float bombRadius = 4f;

    public bool Hit = false;
    public bool BombExp = false;
    public bool NoTarget = false;

    public Vector3 Startpos, Endpos, flatdir;

    private Rigidbody rb;
    private ParticleSystem Effect;


    public void Init(int Type, Transform Target, float Speed, float Damage, float LifeTime,bool NoTarget)
    {
        rb = GetComponent<Rigidbody>();
        Hit = true;
        this.Type = Type;
        this.Target = Target;
        this.Speed = Speed;
        this.Damage = Damage;
        this.Lifetime = LifeTime;
        this.NoTarget = NoTarget;

        if (Type == 0 || Type == 2)
        {
            Vector3 Dir = Target ? (Target.position - transform.position) : transform.forward;
            rb.velocity = Dir * Speed;

            transform.rotation = Quaternion.LookRotation(Dir);
        } else if (Type == 1)
        {
            BoomSet();
        }

        StartCoroutine(HitClear());
    }

    IEnumerator HitClear()
    {
        yield return new WaitForSeconds(0.2f);
        Hit = true;
    }



    private void FixedUpdate()
    {

        currTime += Time.fixedDeltaTime * GameManager.Instance.GameTime * GameManager.Instance.ProjectileTime;

        if (currTime > Lifetime)
        {
            if (Type == 1)
            {
                BoomHit();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        Move();
    }


    void BoomSet()
    {
        Effect = transform.Find("Effect").GetComponent<ParticleSystem>();
        float RandX = Random.Range(-3f, 3f);
        float RandZ = Random.Range(-3f, 3f);

        Startpos = new Vector3(transform.position.x, 0f, transform.position.z);
        Endpos = Target.position + new Vector3(RandX, 0f, RandZ);

        float dist = Vector3.Distance(Startpos, Endpos);
        float SpeedMult = Mathf.Clamp(dist / 20, 0.25f, 1f);

        Speed *= SpeedMult;

        flatdir = new Vector3(-(Endpos - Startpos).normalized.z, 0f, (Endpos - Startpos).normalized.x);
        Lifetime = Vector3.Distance(Startpos, Endpos) / Speed;
        currTime = 0f;
    }

    void Move()
    {
        if (Type == 0 || Type == 2)
        {
            Vector3 Dir = !NoTarget ? (Target.position - transform.position) : transform.forward;
            rb.velocity = Dir * Speed * GameManager.Instance.GameTime * GameManager.Instance.ProjectileTime;

            transform.rotation = Quaternion.LookRotation(Dir);
        } else if (Type == 1)
        {
            float t = Mathf.Clamp01(currTime / Lifetime);
            Vector3 pos = Vector3.Lerp(Startpos, Endpos, t) + flatdir * arkhegiht * t * (1 - t) * 4f;
            pos.y = 0f;
            if (pos - rb.position != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(pos - rb.position);
            rb.MovePosition(pos);
            if (t >= 1)
            {
                BoomHit();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Hit)
            return;

        Unit unit = null;

        if(other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            unit = other.gameObject.GetComponent<Unit>();
            Hit = true;
        }


        if (unit == null) return;


        if (Type == 0 ||  Type == 2)
        {
            unit.Damage(Damage);
            Destroy(gameObject);
        } else if(Type == 1)
        {
            BoomHit();
        }
    }

    void BoomHit()
    {
        if(BombExp)
            return;

        Hit = true;
        BombExp = true;

        Effect.Play();
        Collider[] col = Physics.OverlapSphere(transform.position, bombRadius);

        List<Unit> Aya = new List<Unit>();
        foreach(Collider c in col)
        {
            Unit unit = c.GetComponent<Unit>();
            if(unit != null && unit.isAlive && !Aya.Contains(unit))
            {
                unit.Damage(Damage);
                Aya.Add(unit);
            }
        }

        Renderer material = GetComponent<Renderer>();
        material.enabled = false;

        Destroy(gameObject, 0.6f);
    }
}
