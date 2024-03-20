using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _reflectionRandomanzation;
    
    private Vector2 lastRegidbodyVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        SendBallInRandom();
    }

    private void SendBallInRandom()
    {
        _rigidbody2D.velocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * _speed;
        lastRegidbodyVelocity = _rigidbody2D.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ResetBall();
        }

        if (_rigidbody2D.velocity.magnitude < .1f)
        {
            ResetBall();
        }
    }

    void ResetBall()
    {
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.simulated = false;
        _rigidbody2D.transform.position = Vector3.zero;
        _rigidbody2D.simulated = true;
        SendBallInRandom();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var randomOffset = new Vector2(Random.Range(-_reflectionRandomanzation, _reflectionRandomanzation),
            Random.Range(-_reflectionRandomanzation, _reflectionRandomanzation));
        var reflectedVector = Vector2.Reflect(lastRegidbodyVelocity, (other.contacts[0].normal + randomOffset).normalized);
        _rigidbody2D.velocity = reflectedVector;
        lastRegidbodyVelocity = _rigidbody2D.velocity;
    }
}
