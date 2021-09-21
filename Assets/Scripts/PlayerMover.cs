using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    // Start is called before the first frame update
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Transform _groundchecker;
    [SerializeField] private float _groundcheckerRadius;
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private Collider2D _headCollider;
    [SerializeField] private float _headCheckerRadius;
    [SerializeField] private Transform _headChecker;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crouchAnimatorKey;
    [SerializeField] private Transform _transform;
    
    [Header(("Animation"))] 
    [SerializeField]
    private Animator _animator;
    
    private float _direction;
    private bool _Jump;
    private bool _crawl;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        _animator.SetFloat(_runAnimatorKey, Mathf.Abs(_direction));
        if (_direction > 0 && _spriteRenderer.flipX)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_direction < 0 && !_spriteRenderer.flipX)
        { 
            _spriteRenderer.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _Jump = true;
        }

        _crawl = Input.GetKey(KeyCode.C);

    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);
       //_rigidbody.AddForce(new Vector2(50*_direction,0),ForceMode2D.Impulse);
       //_rigidbody.MovePosition(_rigidbody.position+new Vector2(_direction*1,0));
       //_transform.position += new Vector3(_direction*1,0,0);
       //_transform.Translate(new Vector3(1*_direction,0,0));
       
        
        bool canJump = Physics2D.OverlapCircle(_groundchecker.position, _groundcheckerRadius, _whatIsGround);
        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsGround);

        _headCollider.enabled = !_crawl && canStand;
        
        if (_Jump && canJump)
        {
            _rigidbody.AddForce((Vector2.up * _jumpForce));
            _Jump = false;
        }

        if (!_headCollider.enabled)
        {
           
        }
        
        _animator.SetBool(_jumpAnimatorKey,!canJump);
        _animator.SetBool(_crouchAnimatorKey,!_headCollider.enabled);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundchecker.position, _groundcheckerRadius);
        Gizmos.color= Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
    }

    public void AddHp(int hpPoints)
    {
        Debug.Log("Hp raised" + hpPoints);
    }
    public void AddMp(int mpPoints)
    {
        Debug.Log("Mp raised" + mpPoints);
    }
    public void AddArmor(int armorPoints)
    {
        Debug.Log("more armor " + armorPoints);
    }
}
