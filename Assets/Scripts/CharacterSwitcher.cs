using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab1;
    [SerializeField]
    private GameObject playerPrefab2;

    private void Awake()
    {
        var gamepads = Gamepad.all;
        var keyboards = InputSystem.devices;

        if (gamepads.Count == 1)
        {
            // First player gets the gamepad
            var p1Devices = new InputDevice[] { gamepads[0] };
            var p1 = PlayerInput.Instantiate(playerPrefab1, controlScheme: "Gamepad", pairWithDevices: p1Devices);
            Debug.Log("Player 1 paired with Gamepad: " + gamepads[0]);

            // Second player gets the keyboard
            var p2Devices = new InputDevice[] { keyboards[0] };
            var p2 = PlayerInput.Instantiate(playerPrefab2, controlScheme: "KeyboardMouse", pairWithDevices: p2Devices);
            Debug.Log("Player 2 paired with Keyboard: " + keyboards[0]);
        }
        else if (gamepads.Count == 0)
        {
            // Both players get keyboards
            var p1Devices = new InputDevice[] { keyboards[0] };
            var p1 = PlayerInput.Instantiate(playerPrefab1, controlScheme: "KeyboardMouse", pairWithDevices: p1Devices);
            Debug.Log("Player 1 paired with Keyboard: " + keyboards[0]);

            var p2Devices = new InputDevice[] { keyboards[0] };
            var p2 = PlayerInput.Instantiate(playerPrefab2, controlScheme: "KeyboardMouse", pairWithDevices: p2Devices);
            Debug.Log("Player 2 paired with Keyboard: " + keyboards[1]);
        }
        else if (gamepads.Count >= 2)
        {
            // Both players get their respective gamepad and keyboard
            var p1Devices = new InputDevice[] { gamepads[0], keyboards[0] };
            var p1 = PlayerInput.Instantiate(playerPrefab1, controlScheme: "Gamepad", pairWithDevices: p1Devices);
            Debug.Log("Player 1 paired with Gamepad: " + gamepads[0] + " and Keyboard: " + keyboards[0]);

            var p2Devices = new InputDevice[] { gamepads[1], keyboards[0] };
            var p2 = PlayerInput.Instantiate(playerPrefab2, controlScheme: "Gamepad", pairWithDevices: p2Devices);
            Debug.Log("Player 2 paired with Gamepad: " + gamepads[1] + " and Keyboard: " + keyboards[1]);
        }
        else
        {
            Debug.LogWarning("Not enough input devices available.");
        }
    }
}