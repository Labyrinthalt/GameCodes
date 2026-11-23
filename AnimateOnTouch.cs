using UnityEngine;

public class AnimateOnTouch : MonoBehaviour
{
    public Animator anim;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Activate", true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("Activate", false);
        }
    }
}
