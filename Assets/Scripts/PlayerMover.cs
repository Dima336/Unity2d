using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private Transform _shootPosition;
    [SerializeField] private GameObject _fireBoal;
    [SerializeField] private Transform _transform;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _maxMp;

    [Header(("Animation"))] 
    [SerializeField]
    private Animator _animator;

    [Header("UI")] 
    [SerializeField]
    private TMP_Text _coinValueText;
    
    [Header("UI")] 
    [SerializeField] private TMP_Text _coinsAmountText;

    [SerializeField] private Slider _hpBar;

    [SerializeField] private Slider _mpBar;
    
    
    private float _direction;
    private bool _Jump;
    private bool _crawl;
    private bool _atack;
    private int _currentHp;
    private int _currentMp;

    private int _coinsValue;

    private float _lastPushTime;
    
    public int CoinsValue
    {
        get=>  _coinsValue;
        
        set
        {
            _coinsValue = value;
            _coinValueText.text = value.ToString();
        }
        
    }
    private int CurrentHp
    {
        get => _currentHp;
        set
        {
            if (value > _maxHp)
                value = _maxHp;
            _currentHp = value;
            _hpBar.value = value;
        }
    }
    private int CurrentMp
    {
        get => _currentMp;
        set
        {
            if (value > _maxMp)
                value = _maxMp;
            _currentMp = value;
            _mpBar.value = value;
        }
    }
    private void Start()
    {
        _hpBar.maxValue = _maxHp;
        _mpBar.maxValue = _maxMp;
        CurrentHp = _maxHp;
        CurrentMp = 50;// CurrentMp= _maxMp
        CoinsValue = 0;
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

        if (Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(_fireBoal, _shootPosition.position, transform.rotation);
        }
  

    }

    private void FixedUpdate()
    {
        bool canJump = Physics2D.OverlapCircle(_groundchecker.position, _groundcheckerRadius, _whatIsGround);
        if (_animator.GetBool(_hurtAnimatorKey))
        {
            if (Time.time - _lastPushTime > 0.2f && canJump)
                _animator.SetBool(_hurtAnimatorKey, false);
            return;
        }
        _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);
       //_rigidbody.AddForce(new Vector2(50*_direction,0),ForceMode2D.Impulse);
       //_rigidbody.MovePosition(_rigidbody.position+new Vector2(_direction*1,0));
       //_transform.position += new Vector3(_direction*1,0,0);
       //_transform.Translate(new Vector3(1*_direction,0,0));
       
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
        int missingHp = _maxHp - CurrentHp;
        int pointToAddHp = missingHp > hpPoints ? hpPoints : missingHp;
        StartCoroutine(RestoreHP(pointToAddHp));
    }
    public void AddMp(int mpPoints)
    {
        int missingMp = _maxMp - CurrentMp;
        int pointToAddMp = missingMp > mpPoints ? mpPoints : missingMp;
        StartCoroutine(RestoreMP(pointToAddMp));
    }

    private IEnumerator RestoreHP(int pointToAddHp)
    {
        
        while ( pointToAddHp!=0)
        {
            pointToAddHp--;
            CurrentHp++;
            yield return new WaitForSeconds(0.2f);
        }
    }
    private IEnumerator RestoreMP(int pointToAddMp)
    {
        
        while ( pointToAddMp!=0)
        {
            pointToAddMp--;
            CurrentMp++;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void AddArmor(int armorPoints)
    {
        Debug.Log("more armor " + armorPoints);
    }
    public void TakeDamage(int damage, float pushPower=0, float enemyPosX=0)
    {
        if (_animator.GetBool(_hurtAnimatorKey))
        {
            return;
        }
        CurrentHp -= damage;
        print(CurrentHp);
        if (CurrentHp <= 0)
        {
            gameObject.SetActive(false);
            Invoke(nameof(ReloadScene), 1f);
        }

        if (pushPower != 0 && Time.time - _lastPushTime > 0.5f)
            {
                _lastPushTime = Time.time;
                int direction = transform.position.x > enemyPosX ? 1 : -1;
                _rigidbody.AddForce(new Vector2(direction*pushPower/2,pushPower));
                Debug.Log(pushPower);
                _animator.SetBool(_hurtAnimatorKey,true);
            }
        
    }
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
}
