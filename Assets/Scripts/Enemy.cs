using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject deathExplosionEffect;
    [SerializeField] GameObject hitExplosionEffect;
    Transform parent;
    [SerializeField] [Range(1, 50)] int pointValue;
    [SerializeField] int hitPoints = 1;
    [SerializeField] Material hitMaterial;

    Scoreboard scoreboard;

    private int hitsTaken;

    private void Start()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        hitsTaken = 0;
        AddRigidbody();

        parent = GameObject.FindWithTag("Trash").transform;
    }

    private void AddRigidbody()
    {
        var rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void KillEnemy()
    {
        scoreboard.IncreaseScore(pointValue);
        GameObject vfx = Instantiate(deathExplosionEffect, transform.position, Quaternion.identity);
        vfx.transform.parent = parent;
        PlayExplosionSound();
        Destroy(gameObject);
    }

    private void PlayExplosionSound()
    {
        var soundGameObject = GameObject.FindGameObjectWithTag("ExplosionSound");
        var audio = soundGameObject.GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip, .7f);
    }

    private void ProcessHit()
    {
        hitsTaken++;

        var renderer = transform.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = hitMaterial;
        }

        if (hitsTaken == hitPoints)
        {
            KillEnemy();
        }
        else
        {
            GameObject vfx = Instantiate(hitExplosionEffect, transform.position, Quaternion.identity);
            vfx.transform.parent = parent;
        }
    }
}
