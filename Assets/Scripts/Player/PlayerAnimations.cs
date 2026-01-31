using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimations : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    private PlayerMovement pm;
    private HealthManager healthManager;
    private float animSpeedCorrector = 10f;
    private Transform otherPlayer;
    public Animator playerBodyAnim;
    public AudioSource gunSource;
    public AudioClip gunClip;
    public PlayerInputSelector playerInputSelector;
    private void Start()
    {
        anim = GetComponentsInChildren<Animator>()[1];
        pm = GetComponent<PlayerMovement>();


    }

    private void Update()
    {
        if (!playerInputSelector.selectedInput) return;

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

        if (!otherPlayer)
        {
            try
            {
                if (GetComponent<HealthManager>().playerIndex == 0)
                {
                    otherPlayer = GameObject.FindGameObjectWithTag("Player2").transform;
                }
                else
                {
                    otherPlayer = GameObject.FindGameObjectWithTag("Player1").transform;
                }
            }
            catch (Exception ex) { }

            if(otherPlayer)
            playerBodyAnim = otherPlayer.GetComponentsInChildren<Animator>()[0];
        }

        if (!otherPlayer) return;
        CheckOtherPlayerDirection();

    }

    public void ShootAnim() 
    {
        anim.SetTrigger("Shoot");
        gunSource.PlayOneShot(gunClip);
    }

    private void CheckOtherPlayerDirection()
    {

        playerBodyAnim.transform.LookAt(transform.position);


        Vector3 dirObjeto = transform.InverseTransformDirection(otherPlayer.forward);
        float dotFrente = Vector3.Dot(dirObjeto, Vector3.forward);
        float dotLateral = Vector3.Dot(dirObjeto, Vector3.right);

        if (dotFrente > 0.5f)
        {
            playerBodyAnim.SetInteger("direccion",0);
        }
        else if (dotFrente < -0.5f)
        {
            playerBodyAnim.SetInteger("direccion", 1);

        }
        else
        {
            if (dotLateral > 0.5f)
            {
                playerBodyAnim.SetInteger("direccion", 3);

            }
            else if (dotLateral < -0.5f)
            {
                playerBodyAnim.SetInteger("direccion", 2);

            }
            else
            {
                playerBodyAnim.SetInteger("direccion", 0);
            }
        }
    }



}
