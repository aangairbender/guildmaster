using Gameplay.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;

namespace Gameplay.Characters
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CharacterUI : MonoBehaviour
    {
        [Inject] CameraService cameraService;

        TextMeshProUGUI tmp;

        Character character = null;

        private void Start()
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (cameraService.RaycastMousePosition(~0, out var hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    var character = hit.transform.gameObject.GetComponent<CharacterView>()?.Character;
                    if (character != null)
                    {
                        this.character = character;
                    }
                }
            }

            DisplayCharacter(character);
        }

        private void DisplayCharacter(Character character)
        {
            tmp.text = "";
            if (character == null)
            {
                return;
            }
            tmp.text += $"Task: {character.Task?.GetType()?.Name}\n";
            if (character.CarriedItem != null && character.CarriedQuantity > 0)
            {
                tmp.text += $"Carrying: {character.CarriedQuantity} {character.CarriedItem?.Name}\n";
            }
        }
    }
}
