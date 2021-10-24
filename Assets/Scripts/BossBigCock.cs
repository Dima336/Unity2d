using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBigCock : MonoBehaviour
{
    [SerializeField] private float _walkRange;
    [SerializeField] private bool _face_Right;
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;
    [SerializeField] private int _maxHp;
    [SerializeField] private Slider _slider;
    private int _currentHp;
    private Vector2 _startPosition;
    private int _direction=1;
    private float _lastAtackTime;
    
    
    private int CurrentHp
    {
        get => _currentHp;
        set
        {
            _currentHp = value;
            _slider.value = value;
        }
    }
    
    private void ChangeHp(int hp)
    {
        _currentHp = hp;
        if (_currentHp <= 0)
        {
            Destroy(gameObject);
        }
        _slider.value = hp;
    }
    
    private Vector2 _drawPosition
    {
        get
        {
            if (_startPosition == Vector2.zero)
            {
                return transform.position;
            }
            else
            {
                return _startPosition;
            }
        }
    }
  
    
  
    private void Start()
    {
        _slider.maxValue = _maxHp;
        CurrentHp = _maxHp;
        _startPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPosition, new Vector3(_walkRange*2,_walkRange ,0));
    }

    private void Update()
    {
        if (_face_Right && transform.position.x > _startPosition.x + _walkRange)
        {
            Flip();
        }
        else if (!_face_Right && transform.position.x < _startPosition.x - _walkRange)
        {
            Flip();
        }
    }

    private void Flip()
    {
        _face_Right = !_face_Right;
        transform.Rotate(0,180,0);
        _direction *= -1;
    }
    private void FixedUpdate()
    {
        _rigidbody2D.velocity=Vector2.right * _direction * _speed;

    }

    

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMover player = other.collider.GetComponent<PlayerMover>();
        if (player != null && Time.time - _lastAtackTime > 0.2f)
        {
            _lastAtackTime = Time.time;
            player.TakeDamage(_damage,_pushPower,transform.position.x);
        }
    }
    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            Destroy(gameObject);
        }
      
    }

    public void TakeDamageEnemy(int damage)
    {
        ChangeHp(_currentHp-damage);
    }
    
    
}