using UnityEngine;

public class PlayerShoots2 : MonoBehaviour
{
    public Transform Gun;

    private Vector2 Direction;
    public GameObject Bullet;
    public Animator GunAnimator;
    public float BulletSpeed;
    public Transform ShootPoint;
    public float FireRate;
    float ReadyForNextShot;

    [Header("Sound Effects")]
    [SerializeField] private string weaponSoundName; // Set per weapon in Inspector (leave empty for silent)

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Direction = mousePos - (Vector2)Gun.position;
        FaceMouse();

        if (Input.GetMouseButton(0))
        {
            if (Time.time > ReadyForNextShot)
            {
                if (InventoryController.Instance != null && InventoryController.Instance.ConsumeAmmo(1))
                {
                    ReadyForNextShot = Time.time + 1 / FireRate;
                    Shoot();
                }
            }
        }
    }

    void FaceMouse()
    {
        Gun.transform.right = Direction;
    }

    void Shoot()
    {
        GameObject BulletIns = Instantiate(Bullet, ShootPoint.position, ShootPoint.rotation);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(BulletIns.transform.right * BulletSpeed);
        GunAnimator.SetTrigger("Shoots");
        Destroy(BulletIns, 2f);

        PlayWeaponSound();
    }

    private void PlayWeaponSound()
    {
        if (string.IsNullOrWhiteSpace(weaponSoundName))
            return;

        SoundEffectManager.Play(weaponSoundName);
    }
}