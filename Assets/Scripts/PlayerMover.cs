using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _speed;

    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _jumpForce;

    [SerializeField] private Transform _groundChecker;

    [SerializeField] private float _groundCheckerRadius;

    [SerializeField] private LayerMask _whatIsGround;

    [SerializeField] private LayerMask _whatIsCell;

    [SerializeField] private Collider2D _headCollider;

    [SerializeField] private float _headCheckerRadius;

    [SerializeField] private Transform _headChecker;



    [Header(("Animation"))]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _runAnimatorKey;
    [SerializeField] private string _jumpAnimatorKey;
    [SerializeField] private string _crawlAnimatorKey;
    [SerializeField] private string _hurtAnimatorKey;
    [SerializeField] private string _attackAnimatorKey;


    [SerializeField] private int _maxHp;
   private int _currentHp;

   [SerializeField] private int _maxMana;
    private int _currentMana;


   [Header("UI")]
   [SerializeField] private TMPro.TMP_Text _coinsAmountText;
   [SerializeField] private Slider _hpBar;
   [SerializeField] private Slider _ManaBar;


    [Header("Attack")]
    [SerializeField] private int _swordDamage;
    [SerializeField] private Transform _swordAttackPoint;
    [SerializeField] private float _swordAttackRadius;
    [SerializeField] private LayerMask _WhatIsEnemy;

    private float _verticalDirection;
    private float _horizontalDirection;
    private bool _Jump;
    private bool _crawl;
    private bool _needToAttack;
  


    private float _lastPushTime;

    private int _coinsAmount;
    public int CoinsAmount
    {
        get => _coinsAmount;
        set
        {
            _coinsAmount = value;
            _coinsAmountText.text = value.ToString();
        }
    }
   private int CurrentHp
    {
        get => _currentHp;

        set
        {
            if (value > _maxHp)
            {
                value = _maxHp;
            }
            _currentHp = value;
            _hpBar.value = value;
        }
    }
    private int CurrentMana
    {
        get => _currentMana;

        set
        {
            if (value > _maxMana)
            {
                value = _maxMana;
            }
            _currentMana = value;
            _ManaBar.value = value;
        }
    }
    
    public bool CanClimb { private get; set; }
    //private bool _attack;

    // Start is called before the first frame update
    private void Start()
    {
       CoinsAmount = 0;

        _ManaBar.maxValue = _maxMana;
        CurrentMana = _maxMana;

        _hpBar.maxValue = _maxHp;
     CurrentHp = _maxHp;

        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        {
            if(Input.GetButtonDown("Fire1"))
            {
                _needToAttack = true;
            }
            _horizontalDirection = Input.GetAxisRaw("Horizontal");
            _verticalDirection = Input.GetAxisRaw("Vertical");


            _animator.SetFloat(_runAnimatorKey, Mathf.Abs(_horizontalDirection));
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _Jump = true;
            }

            if (_horizontalDirection > 0 && _spriteRenderer.flipX) //bool - true/false
            {
                _spriteRenderer.flipX = false;
            }
            else if (_horizontalDirection < 0 && !_spriteRenderer.flipX) //! - no 
            {
                _spriteRenderer.flipX = true;
            }

            _crawl = Input.GetKey(KeyCode.C);
            /* if (Input.GetKeyDown(KeyCode.V))
             {
                 _attack = true;
             }
            */

        }

    }
    private void FixedUpdate()
    {
        
        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);

        if (_animator.GetBool(_hurtAnimatorKey))
        {
            if(Time.time - _lastPushTime> 0.2f && canJump)
            {
                _animator.SetBool(_hurtAnimatorKey, false);
            }
            return;
        }
        _rigidbody.velocity = new Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);

        if (CanClimb)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _verticalDirection * _speed);
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = 1;
        }

       
        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsCell);
        Collider2D coll = Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsCell);


        _headCollider.enabled = !_crawl && canStand;
        if (_Jump && canJump)
        {

            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _Jump = false;

        }



        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crawlAnimatorKey, !_headCollider.enabled);
        //_animator.SetBool(_attackAnimatorKey, _attack);

        if(_animator.GetBool(_hurtAnimatorKey))
        {

        }

        if(_needToAttack)
        {
            StartAttack();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_swordAttackPoint.position, new Vector3(_swordAttackRadius, _swordAttackRadius, 0));
    }

    private void StartAttack()
    {
        if(_animator.GetBool(_attackAnimatorKey))
        {
            return;
        }
        _animator.SetBool(_attackAnimatorKey, true);
    }

    private void Attack()
    {
        Collider2D[] targets = Physics2D.OverlapBoxAll(_swordAttackPoint.position, 
            new Vector2(_swordAttackRadius, _swordAttackRadius), _WhatIsEnemy);
        foreach(var target in targets)
        {
            Ranged_enemy rangedEnemy = target.GetComponent<Ranged_enemy>();
            if(rangedEnemy != null)
            {
                rangedEnemy.TakeDamage(_swordDamage);
            }
        }
        _animator.SetBool(_attackAnimatorKey, false);
        _needToAttack = false;
    }
   public void AddHp(int hpPoints)
    {
        int missingHp = _maxHp - CurrentHp;
        int pointsToAdd = missingHp > hpPoints ? hpPoints : missingHp;
        StartCoroutine(RestoreHp(pointsToAdd));
    }


    private IEnumerator RestoreHp(int pointsToAdd)
    {

        while (pointsToAdd != 0)
        {
            pointsToAdd--;
            CurrentHp++;
            yield return new WaitForSeconds(0.2f);
        }
    }
   public void AddMana(int ManaPoints)
    {
        int missingMana = _maxMana - CurrentMana;
        int ManaToAdd = missingMana > ManaPoints ? ManaPoints : missingMana;
        StartCoroutine(RestoreMana(ManaToAdd));
    }
  

    private IEnumerator RestoreMana(int ManaToAdd)
    {

        while (ManaToAdd != 0)
        {
            ManaToAdd--;
            CurrentMana++;
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void TakeDamage(int damage, float pushPower = 0, float enemyPosX = 0)
    {
     if(_animator.GetBool(_hurtAnimatorKey))
        {
            return;
        }

        CurrentHp -= damage;
        if (_currentHp <= 0)
        {
            Debug.Log("Died");
            gameObject.SetActive(false);
            Invoke(nameof(ReloadScene), 1f);
        }

        if(pushPower !=0)
        {
            _lastPushTime = Time.time;
            int _direction = transform.position.x > enemyPosX ? 1 : -1;
            _rigidbody.AddForce(new Vector2(_direction * pushPower / 2, pushPower));
          _animator.SetBool(_hurtAnimatorKey, true);
        }

    }
    public void TakeManaEater(int eater)
    {
        CurrentMana -= eater;
        if (_currentMana <= 0)
        {
            Debug.Log("Low Mana");
        }

    }
    public void OpenChest(int chest)
    {
        Debug.Log($"You earned gold: {chest}");
    }
    public void OpenBox(int box)
    {
        Debug.Log($"You found food: {box}");
    }
   /* public void AddMana(int ManaPotion)
    {
        Debug.Log($"Mana raised: {ManaPotion}");
    }*/
    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}