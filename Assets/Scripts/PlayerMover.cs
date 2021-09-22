using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField]private float _speed;

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
    //[SerializeField] private string _attackAnimatorKey;

    private float _verticalDirection;
    private float _horizontalDirection;
    private bool _Jump;
    private bool _crawl;

    public bool CanClimb { private get; set; }
    //private bool _attack;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        {
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
        _rigidbody.velocity = new Vector2(_horizontalDirection * _speed, _rigidbody.velocity.y);

        if(CanClimb)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _verticalDirection * _speed);
            _rigidbody.gravityScale = 0;
        }
        else
        {
            _rigidbody.gravityScale = 1;
        }

        bool canJump = Physics2D.OverlapCircle(_groundChecker.position, _groundCheckerRadius, _whatIsGround);
        bool canStand = !Physics2D.OverlapCircle(_headChecker.position, _headCheckerRadius, _whatIsCell);



        _headCollider.enabled = !_crawl && canStand;
        if (_Jump && canJump)
        {

            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _Jump = false;

        }

        

        _animator.SetBool(_jumpAnimatorKey, !canJump);
        _animator.SetBool(_crawlAnimatorKey, !_headCollider.enabled);
        //_animator.SetBool(_attackAnimatorKey, _attack);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundChecker.position, _groundCheckerRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_headChecker.position, _headCheckerRadius);
    }
    public void AddHp(int hpPoints)
    {
        Debug.Log($"Hp raised: {hpPoints}");
    }
    public void OpenChest(int chest)
    {
        Debug.Log($"You earned gold: {chest}");
    }
    public void OpenBox(int box)
    {
        Debug.Log($"You found food: {box}");
    }
    public void AddMana(int ManaPotion)
    {
        Debug.Log($"Mana raised: {ManaPotion}");
    }
}
