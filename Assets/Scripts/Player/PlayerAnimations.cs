using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimations : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    private PlayerMovement pm;
    private float animSpeedCorrector = 10f;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        anim.speed = pm.speed / animSpeedCorrector;

        if (pm.isMoving())
        {
            if ((pm.isGrounded || pm.wallRunning || pm.climbing) && !pm.sliding)
            {
                anim.SetBool("Move", true);
            }
            else 
            {
                anim.SetBool("Move", false);
            }
        }
        else 
        {
            anim.SetBool("Move", false);
        }

    }

    public void ShootAnim() 
    {
        anim.SetTrigger("Shoot");
    }


}
