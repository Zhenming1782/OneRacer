using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    MeshRenderer _renderer;
    float _maxSpeed = 2f, _currentSpeed;
    const float _maxDistance = 100f;
    // Start is called before the first frame update
    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (transform.name == "Road")
        {
            _currentSpeed = (1-gameObject.transform.position.z*2f/_maxDistance) * _maxSpeed* 2f;
        }
        else
        {
            _currentSpeed = (1-gameObject.transform.position.z/ _maxDistance) * _maxSpeed;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        _renderer.material.mainTextureOffset = new Vector2(Time.time * _currentSpeed, 0f);
    }
}
