using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    public GameObject impactEffect;

    public float speed = 70f;
    public float explosionRadius = 0;
    private Turret turret;
    public GameObject forceFieldPrefab;
    private bool hasSpawnedForceField;
    private bool hasUsedEffect = false;


    public int damage = 50;


    public void Seek(Transform _target)
    {
        target = _target;
    }

    public void setParent(Turret t)
    {
        turret = t;
    }


    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (!hasSpawnedForceField)
            {
                Destroy(gameObject);
            }
            turret.fireCountdown = (1f / turret.fireRate) / 2;
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
        
    }

    void HitTarget()
    {
        if (hasUsedEffect) return;
        if (GraphicsManager.particles)
        {
            GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
        }
        hasUsedEffect = true;

        if (explosionRadius > 0f)
        {
            Explode();
            Destroy(gameObject);

        } else if (!forceFieldPrefab)
        {
            Damage(target, damage);
            Destroy(gameObject);
        } else if (!hasSpawnedForceField && !target.GetComponent<Enemy>().isFINALBOSS)
        {
            StartCoroutine(CreateForceField());
            hasSpawnedForceField = true;
            transform.localScale = new Vector3(0, 0, 0);
            transform.position = new Vector3(0,-100,0);
        }
        
    }



    IEnumerator CreateForceField()
    {
        AnimationClip clip = new();
        clip.legacy = true;
        GameObject forceField = Instantiate(forceFieldPrefab, transform.position, transform.rotation);
        float endTime = turret.animationSpeed;
        Keyframe[] keyX = new Keyframe[2];
        keyX[0] = new Keyframe(0f, 0f);
        keyX[1] = new Keyframe(endTime, turret.blastRadius);

        Keyframe[] keyY = new Keyframe[2];
        keyY[0] = new Keyframe(0f, 0f);
        keyY[1] = new Keyframe(endTime, turret.blastRadius);

        Keyframe[] keyZ = new Keyframe[2];
        keyZ[0] = new Keyframe(0f, 0f);
        keyZ[1] = new Keyframe(endTime, turret.blastRadius);
        AnimationCurve curveX = new(keyX);
        AnimationCurve curveY = new(keyY);
        AnimationCurve curveZ = new(keyZ);
        clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
        clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
        clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
        Animation anim = forceField.GetComponent<Animation>();
        clip.name = "Expand";
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);

        yield return new WaitForSeconds(endTime);
        yield return ForceFieldDamage(forceField);

    }

    IEnumerator ForceFieldDamage(GameObject forceField)
    {
        float timeElapsed = 0f;
        while (timeElapsed <= turret.forceFieldLife) {
            timeElapsed += Time.deltaTime;
            Collider[] colliders = Physics.OverlapSphere(forceField.transform.position, turret.blastRadius/2);
            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Enemy")
                {
                    Damage(collider.transform, Mathf.RoundToInt(turret.damagePerSecond*Time.deltaTime));
                    collider.gameObject.GetComponent<Enemy>().Slow(turret.slowPercentForceField);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        AnimationClip clip = new();
        clip.legacy = true;
        float endTime = turret.animationSpeed;
        Keyframe[] keyX = new Keyframe[2];
        keyX[0] = new Keyframe(0f, turret.blastRadius);
        keyX[1] = new Keyframe(endTime, 0f);

        Keyframe[] keyY = new Keyframe[2];
        keyY[0] = new Keyframe(0f, turret.blastRadius);
        keyY[1] = new Keyframe(endTime, 0f);

        Keyframe[] keyZ = new Keyframe[2];
        keyZ[0] = new Keyframe(0f, turret.blastRadius);
        keyZ[1] = new Keyframe(endTime, 0f);
        AnimationCurve curveX = new(keyX);
        AnimationCurve curveY = new(keyY);
        AnimationCurve curveZ = new(keyZ);
        clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
        clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
        clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
        Animation anim = forceField.GetComponent<Animation>();
        clip.name = "Destroy";
        anim.AddClip(clip, clip.name);
        anim.Play(clip.name);

        yield return new WaitForSeconds(endTime);



        Destroy(forceField);
        Destroy(gameObject);
    }



    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform, damage);
            }
        }
    }


    void Damage(Transform enemy, int damage)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }

    }
}
