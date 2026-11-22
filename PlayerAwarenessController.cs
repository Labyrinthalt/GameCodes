using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }
    // Totoong distansya (hindi normalized), gamitin ito sa attack range
    public float DistanceToPlayer { get; private set; }

    [SerializeField]
    private float _playerAwarenessDistance = 6f;

    // Itsek para makita sa Inspector habang nagpe-play (pang-debug)
    [SerializeField]
    private bool _debugAware;

    private Transform _player;

    private void Awake()
    {
        PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
            _player = playerMovement.transform;
        else
            Debug.LogError("PlayerAwarenessController: Walang PlayerMovement na nahanap sa scene!", this);
    }

    private void Update()
    {
        if (_player == null) return;

        Vector2 enemyToPlayerVector = _player.position - transform.position;
        DistanceToPlayer = enemyToPlayerVector.magnitude;
        DirectionToPlayer = enemyToPlayerVector.normalized;

        AwareOfPlayer = DistanceToPlayer <= _playerAwarenessDistance;
        _debugAware = AwareOfPlayer;
    }
}