using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    int totalWeapons = 0;
    public int CurrentWeaponIndex = -1;
    public bool[] unlockedWeapons;

    public GameObject[] Weapons;
    public GameObject WeaponHolder;
    public GameObject CurrentWeapon;

    void Start()
    {
        if (WeaponHolder == null)
        {
            WeaponHolder = gameObject;
        }

        totalWeapons = WeaponHolder.transform.childCount;
        Weapons = new GameObject[totalWeapons];
        unlockedWeapons = new bool[totalWeapons];

        for (int i = 0; i < totalWeapons; i++)
        {
            Weapons[i] = WeaponHolder.transform.GetChild(i).gameObject;
            Weapons[i].SetActive(false);
            unlockedWeapons[i] = false;
        }

        CurrentWeapon = null;
        CurrentWeaponIndex = -1;

        // Optional: If you want the player to start with index 0 unlocked & equipped,
        // uncomment the two lines below:
        // unlockedWeapons[0] = true;
        // SelectWeapon(0);
    }

    void Update()
    {
        if (totalWeapons == 0) return;

        // Next weapon (Key 3) - skips locked weapons
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int nextIndex = FindNextUnlockedIndex(CurrentWeaponIndex, 1);
            if (nextIndex != -1)
            {
                SelectWeapon(nextIndex);
            }
        }

        // Previous weapon (Key 1) - skips locked weapons
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int prevIndex = FindNextUnlockedIndex(CurrentWeaponIndex, -1);
            if (prevIndex != -1)
            {
                SelectWeapon(prevIndex);
            }
        }
    }

    private int FindNextUnlockedIndex(int startIndex, int direction)
    {
        int current = (startIndex < 0) ? (direction > 0 ? -1 : totalWeapons) : startIndex;

        for (int step = 1; step <= totalWeapons; step++)
        {
            int candidate = current + direction * step;
            if (candidate < 0 || candidate >= totalWeapons)
            {
                break; // Stop at the ends of the weapon list (matches your original behavior)
            }

            if (unlockedWeapons[candidate])
            {
                return candidate;
            }
        }

        return -1; // No unlocked weapon in that direction
    }

    public void SelectWeapon(int index)
    {
        if (index < 0 || index >= Weapons.Length) return;
        if (!unlockedWeapons[index]) return;

        if (CurrentWeapon != null)
        {
            CurrentWeapon.SetActive(false);
        }

        CurrentWeaponIndex = index;
        CurrentWeapon = Weapons[index];
        CurrentWeapon.SetActive(true);
    }

    public void UnlockWeapon(string weaponName, bool autoEquip = true)
    {
        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Weapons[i].name == weaponName)
            {
                unlockedWeapons[i] = true;

                if (autoEquip)
                {
                    SelectWeapon(i);
                }

                return;
            }
        }

        Debug.LogWarning("Weapon not found in GunHolder: " + weaponName);
    }
}