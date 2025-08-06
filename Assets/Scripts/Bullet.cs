using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 10;

    private Vector3 direction;
    private float timer;

    public void Shoot(Vector3 dir)
    {
        direction = dir.normalized;
        timer = lifeTime;
        gameObject.SetActive(true);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        NPC target = other.GetComponent<NPC>();
        if (target != null)
        {
            target.TakeDamage(damage);
        }
        gameObject.SetActive(false);
    }
}
