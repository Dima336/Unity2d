using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float _walkRange;
    [SerializeField] private LayerMask _whatIsPlayer;
    [SerializeField] private bool _face_Right;
    [SerializeField] private float _speed;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private int _damage;
    [SerializeField] private float _pushPower;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _jumpRange;
    private Vector2 _startPosition;
    private int _direction=1;
    private float _lastAtackTime;
    private bool _Jump;
    private bool _canJump;

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
        _startPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_drawPosition, new Vector3(_walkRange*2,1,0));
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
    public void CheckIfCanJump()
    {
        Collider2D player = Physics2D.OverlapBox(transform.position, new Vector2(_jumpRange, 1), 0, _whatIsPlayer);
        if (player != null)
        {
            _canJump = true;
            StartJump(player.transform.position);
        }
        else
        {
            _canJump = false;
        }
    }
    private void FixedUpdate()
    {
        if (_canJump)
        {
            _rigidbody2D.AddForce((Vector2.up * _jumpForce));
            _canJump = false;
            return;
        }
        CheckIfCanJump();
        _rigidbody2D.velocity=Vector2.right * _direction * _speed;
    
    }

    public void StartJump(Vector2 playerPosition)
    {
        if (transform.position.x > playerPosition.x && _face_Right ||
            transform.position.x < playerPosition.x && !_face_Right)
        {
            _face_Right = !_face_Right;
            transform.Rotate(0, 180, 0);
        }
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
}