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

            var p1 = PlayerInput.Instantiate(playerPrefab1, controlScheme: "idkbro", pairWithDevice: Gamepad.all[0]);
            var p2 = PlayerInput.Instantiate(playerPrefab2, controlScheme: "idkbro2", pairWithDevice: Gamepad.all[1]);

            Debug.Log(Gamepad.all[0]);
            Debug.Log(Gamepad.all[1]);
    }
    
}