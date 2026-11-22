using System.Collections.Generic;
using UnityEngine;


public static class WorldState
{
    private static readonly HashSet<string> removed = new HashSet<string>();

   
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        removed.Clear();
    }

    public static bool IsRemoved(string id) { return removed.Contains(id); }
    public static void MarkRemoved(string id) { removed.Add(id); }
}

public class WorldObjectId : MonoBehaviour
{
    
    public string Id { get { return gameObject.scene.name + "/" + gameObject.name; } }

    private void Awake()
    {
       
        if (WorldState.IsRemoved(Id)) Destroy(gameObject);
    }

    public void MarkRemoved()
    {
        WorldState.MarkRemoved(Id);
    }
}