using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public Transform Target;

    [Header("공격 설정")]
    public float Range;
    public float StopRange;
    public float AttackTime;
    public float CurrentAttackTime;

    public List<Transform> BulletPos = new List<Transform>();
    public List<Projectile> Projectiles = new List<Projectile>();


    public int Type;
    public float bulletDamage;
    public int MaxBullet;
    public float nextAttack;
    public float bulletSpeed;
    public float bulletLifeTime;
    public bool NoTarget;

    public bool Osoi;
    public bool Out = false;

    private ParticleSystem Effect;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody>();

        Target = Player.Instance.transform;

        Material = GetComponent<Renderer>().material;
        OriginColor = Material.color;

        //Effect = transform.Find("Effect").GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        if (!isAlive || isKnockBack)
            return;

        


        Move();


    }

    private void Update()
    {
        if (!isAlive)
            return;

        Fire();
    }

    void Fire()
    {
        CurrentAttackTime += Time.fixedDeltaTime * GameManager.Instance.GameTime;
        if (Vector3.Distance(transform.position, Target.position) <= Range && CurrentAttackTime > AttackTime)
        {
            CurrentAttackTime = 0f;
            StartCoroutine(Shooting());
        }
    }

    void Move()
    {
        CameraTo();

        Vector3 dir = (Target.position - transform.position).normalized;
        if (Vector3.Distance(transform.position, Target.position) > StopRange || Out)
        {
            rb.velocity = dir * Speed * GameManager.Instance.GameTime;

        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.2f);
        }

        if(dir != Vector3.zero)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, 360f * Time.fixedDeltaTime * GameManager.Instance.GameTime);
        }

    }

    void CameraTo()
    {
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);

        vp.x = Mathf.Clamp01(vp.x);
        vp.y = Mathf.Clamp01(vp.y);

        Vector3 World = Camera.main.ViewportToWorldPoint(vp);

        if (Vector3.Distance(transform.position, World) > 0.01f)
        {
            Out = true;
        } else
        {
            Out = false;
        }

    }



    public virtual IEnumerator Shooting()
    {
        int index = 0;

        if (Osoi)
            StartCoroutine(KnockBack(7f, 0.2f));

        for(int i = 0; i < MaxBullet; i++)
        {
            if(index == BulletPos.Count - 1)
            {
                index = 0;
            }

            Projectile projectile = Instantiate(Projectiles[0], BulletPos[index].position, Quaternion.identity);
            projectile.Init(Type, Target, bulletSpeed, bulletDamage, bulletLifeTime, NoTarget );

            yield return new WaitForSeconds(nextAttack);
        }
    }

    public virtual IEnumerator BulletCircle(int Pnum,int count, float radius, float nextAttackTime = 0f, int Repeat = 1, float rotateSpeed = 0f, float wave = 0f, float bulletSpeed = 5f, float damage = 5f)
    {
        float angle = 0f;

        int num = 0;

        while(num < Repeat)
        {
            num++;

            for(int i =0; i< count; i++)
            {
                float a = angle + (360f / count) * i;
                float r = radius + Mathf.Sin(Time.time + i) * wave;

                Vector3 dir = Quaternion.Euler(0f, a, 0f) * Vector3.forward;
                Vector3 spawnp = transform.position + dir * r;

                Projectile projectile = Instantiate(Projectiles[Pnum], spawnp, Quaternion.LookRotation(dir));
                projectile.Init(2, Target, bulletSpeed, damage, Projectiles[Pnum].Lifetime, Projectiles[Pnum].NoTarget);


            }

            angle += rotateSpeed * nextAttackTime;

            yield return new WaitForSeconds(nextAttackTime);
        }
    }


    public override void Damage(float damage)
    {
        StartCoroutine(HitEffect());
        base.Damage(damage);
    }

    IEnumerator HitEffect()
    {
        Material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        Material.color = OriginColor;
    }

    public override void Death()
    {
        Effect.Play();

        MeshRenderer render = GetComponent<MeshRenderer>();
        render.enabled = false;

        Destroy(gameObject, 0.6f);
    }
}
