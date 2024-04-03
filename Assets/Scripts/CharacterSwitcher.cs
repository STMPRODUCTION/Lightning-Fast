using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. InputSystem;
public class CharacterSwitcher: MonoBehaviour
{
   //public PlayerInputManager playerInputManager;
   [SerializeField]
   private GameObject playerPrefab1;
   [SerializeField]
   private GameObject playerPrefab2;

     private void Awake()
    {
        // Get all connected gamepads
        var connectedGamepads = Gamepad.all;

        if (connectedGamepads.Count == 0)
        {
            // If no gamepad is connected, spawn both players with keyboard control
            PlayerInput.Instantiate(playerPrefab1, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
            PlayerInput.Instantiate(playerPrefab2, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
        }
        else if (connectedGamepads.Count == 1)
        {
            // If only one gamepad is connected, spawn player 1 with gamepad and player 2 with keyboard
            PlayerInput.Instantiate(playerPrefab1, controlScheme: "Player1", pairWithDevice: connectedGamepads[0]);
            PlayerInput.Instantiate(playerPrefab2, controlScheme: "Keyboard", pairWithDevice: Keyboard.current);
        }
        else
        {
            // If multiple gamepads are connected, spawn both players with gamepad control
            PlayerInput.Instantiate(playerPrefab1, controlScheme: "Player1", pairWithDevice: connectedGamepads[0]);
            PlayerInput.Instantiate(playerPrefab2, controlScheme: "Player2", pairWithDevice: connectedGamepads[1]);
        }
    }
    
}