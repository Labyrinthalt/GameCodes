using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraFindPlayer : MonoBehaviour
{
    private IEnumerator Start()
    {
        
        yield return null;

        CinemachineCamera cam = GetComponent<CinemachineCamera>();
        GameObject player = GameObject.FindWithTag("Player");

        if (cam == null || player == null)
        {
            Debug.LogWarning("CameraFindPlayer: walang CinemachineCamera o Player na may tag 'Player'");
            yield break;
        }

        cam.Follow = player.transform;
    }
}