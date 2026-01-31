using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class M_ArrowController : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button leftArrow;
    public Button rightArrow;
    public PlayerInputSelector playerInputSelector;
    private bool selected = false;
    public bool notSelectedNeeded = false;
    private bool wasPressingLeft = false;
    private bool wasPressingRight = false;
    public bool isMenu = false;
    private float deadzone = 0.5f; // Umbral para detectar el input
    private void Update()
    {
        if (selected || notSelectedNeeded)
        {
            float horizontal = 0;


            if (isMenu) 
            {
                horizontal = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Joystick1_Horizontal Controller") + Input.GetAxisRaw("Joystick2_Horizontal Controller");

            }

            if (playerInputSelector != null)
            {
                GameObject player = null;
                if (playerInputSelector.playerIndex == 1) player = GameObject.FindGameObjectWithTag("Player1");

                if (player)
                {
                    PlayerInputSelector otherPlayerInputSelector = player.GetComponentInChildren<PlayerInputSelector>();

                    string[] joystickNames = Input.GetJoystickNames();


                    if ((int)otherPlayerInputSelector.inputDevice == 0)
                    {
                        horizontal = Input.GetAxisRaw("Joystick1_Horizontal Controller") + Input.GetAxisRaw("Joystick2_Horizontal Controller");

                        if (Input.GetButtonDown("Joystick2_Submit"))
                        {
                            if (joystickNames.Length > 2)
                            {

                                if (GetComponent<Button>())
                                    GetComponent<Button>().onClick?.Invoke();
                            }
                        }
                        if (Input.GetButtonDown("Joystick1_Submit"))
                        {
                            if (GetComponent<Button>())
                                GetComponent<Button>().onClick?.Invoke();
                        }
                    }
                    else if ((int)otherPlayerInputSelector.inputDevice == 1)
                    {
                        horizontal = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Joystick2_Horizontal Controller");

                        if (Input.GetButtonDown("Joystick2_Submit"))
                        {
                            if (joystickNames.Length > 2)
                            {

                                if (GetComponent<Button>())
                                    GetComponent<Button>().onClick?.Invoke();
                            }
                        }

                        if (Input.GetButtonDown("Submit"))
                        {
                            if (GetComponent<Button>())
                                GetComponent<Button>().onClick?.Invoke();
                        }
                    }
                    else if ((int)otherPlayerInputSelector.inputDevice == 2)
                    {
                        horizontal = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Joystick1_Horizontal Controller");
                        if ((Input.GetButtonDown("Joystick1_Submit") || Input.GetButtonDown("Submit")))
                        {
                            if (GetComponent<Button>())
                                GetComponent<Button>().onClick?.Invoke();
                        }
                    }
                }
                else 
                {
                    horizontal = Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Joystick2_Horizontal Controller") + Input.GetAxisRaw("Joystick1_Horizontal Controller");
                    if ((Input.GetButtonDown("Joystick1_Submit") || Input.GetButtonDown("Joystick2_Submit") || Input.GetButtonDown("Submit")) && GetComponent<Button>())
                    {
                        GetComponent<Button>().onClick?.Invoke();
                    }
                }
            }

            // Detectar primera pulsación izquierda
            if (horizontal < -deadzone && !wasPressingLeft)
            {
                leftArrow.onClick.Invoke();
                wasPressingLeft = true;
            }
            // Detectar primera pulsación derecha
            else if (horizontal > deadzone && !wasPressingRight)
            {
                rightArrow.onClick.Invoke();
                wasPressingRight = true;
            }

            // Reset cuando el joystick vuelve al centro
            if (Mathf.Abs(horizontal) < deadzone)
            {
                wasPressingLeft = false;
                wasPressingRight = false;
            }



        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selected = false;
    }
}
