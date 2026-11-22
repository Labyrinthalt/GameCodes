using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MapTransation : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner confiner;

    private void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D = mapBoundary;
        }
    }
}
