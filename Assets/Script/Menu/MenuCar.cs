using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuCar : MonoBehaviour
{
    private Vector3 velocity;
    private ParticleSystem particles;

    private void Awake()
    {
        velocity = new Vector3();
        particles = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    void Start()
    {
        Random.InitState((int) DateTime.Now.Ticks);
        velocity.x = Random.Range(5f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity*Time.deltaTime);
        if (transform.localPosition.x > 5f+Mathf.PingPong(Time.time, 5f))
        {
            velocity.x = -Random.Range(5f, 10f);
            particles.startSpeed = 15f+velocity.x;
        }
        else if (transform.localPosition.x < -5f-Mathf.PingPong(Time.time, 5f))
        {
            velocity.x = Random.Range(5f, 10f);
            particles.startSpeed = 15f+velocity.x;
        }
    }
}