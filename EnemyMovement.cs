using UnityEngine;

[RequireComponent(typeof(PlayerAwarenessController))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float resumeBuffer = 0.6f;
    [SerializeField] private float attackHitDelay = 0.3f; // gaano katagal bago tumama ang damage pagkasimula ng attack
    [SerializeField] private bool spriteFacesRight = false;
    [SerializeField] private bool debugLogs = false;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _awareness;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private Enemy _enemy;
    private PlayerHealth _playerHealth;
    private float nextAttackTime;
    private bool _isWalking;
    private bool _inAttackMode;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _awareness = GetComponent<PlayerAwarenessController>();
        _animator = GetComponentInChildren<Animator>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _enemy = GetComponent<Enemy>();
        if (_rigidbody == null)
            Debug.LogWarning($"{name}: Walang Rigidbody2D.", this);
    }

    private void Start()
    {
        _playerHealth = FindAnyObjectByType<PlayerHealth>();
    }

    private void FixedUpdate()
    {
        if (_rigidbody == null) return;

        if (!_awareness.AwareOfPlayer)
        {
            Stop();
            _inAttackMode = false;
            return;
        }

        float dist = _awareness.DistanceToPlayer;
        if (dist <= attackRange) _inAttackMode = true;
        else if (dist > attackRange + resumeBuffer) _inAttackMode = false;

        Vector2 dir = _awareness.DirectionToPlayer;

        if (_sprite != null && !_inAttackMode && Mathf.Abs(dir.x) > 0.3f)
        {
            bool playerIsRight = dir.x > 0f;
            _sprite.flipX = spriteFacesRight ? !playerIsRight : playerIsRight;
        }

        if (_inAttackMode)
        {
            Stop();
            Attack();
        }
        else
        {
            float moveX = Mathf.Sign(dir.x);
            _rigidbody.linearVelocity = new Vector2(moveX * speed, _rigidbody.linearVelocity.y);
            SetWalking(true);
        }
    }

    private void Stop()
    {
        _rigidbody.linearVelocity = new Vector2(0f, _rigidbody.linearVelocity.y);
        SetWalking(false);
    }

    private void SetWalking(bool value)
    {
        if (_animator == null || _isWalking == value) return;
        _isWalking = value;
        _animator.SetBool("IsWalking", value);
    }

    private void Attack()
    {
        if (Time.time < nextAttackTime) return;
        if (debugLogs)
            Debug.Log($"{name} Attack! distance = {_awareness.DistanceToPlayer:F2}", this);
        if (_animator != null)
            _animator.SetTrigger("Attack");

        Invoke(nameof(DealDamage), attackHitDelay);
        nextAttackTime = Time.time + attackCooldown;
    }

    private void DealDamage()
    {
        if (_playerHealth == null || _enemy == null) return;

        // Tatama lang kung nasa abot pa rin si Collin
        if (_awareness.DistanceToPlayer <= attackRange + resumeBuffer)
        {
            _playerHealth.TakeDamage(_enemy.damage);
        }
    }
}