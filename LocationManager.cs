using UnityEngine;

public class LocationManager : MonoBehaviour
{
    [Header("Location roots")]
    public GameObject evergreenLocation;   // EvergreenBG + EvergreenBuildingsFG (group them under one empty parent)
    public GameObject oldGrowthLocation;   // Oldgrowth1BG_0 + Oldgrowth1Buildings_0 (group under one parent)
    public GameObject mapScreen;           // the map UI/object

    [Header("Player")]
    public Transform player;
    public Transform evergreenSpawn;
    public Transform oldGrowthSpawn;

    public void GoToEvergreen() { Show(evergreenLocation, evergreenSpawn); }
    public void GoToOldGrowth() { Show(oldGrowthLocation, oldGrowthSpawn); }

    void Show(GameObject target, Transform spawn)
    {
        evergreenLocation.SetActive(false);
        oldGrowthLocation.SetActive(false);
        if (mapScreen) mapScreen.SetActive(false);

        target.SetActive(true);
        player.position = spawn.position;
    }
}