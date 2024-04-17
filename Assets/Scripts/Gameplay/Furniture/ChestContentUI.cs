using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Gameplay.Furniture
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    internal class ChestContentUI : MonoBehaviour
    {
        TextMeshProUGUI tmp;

        private void Start()
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            var chest = GameWorld.Default?.Chests.FirstOrDefault();
            if (chest != null)
            {
                DisplayContent(chest);
            }
        }

        private void DisplayContent(Chest chest)
        {
            if (chest.Content.Count == 0)
            {
                tmp.text = "chest empty";
                return;
            }

            tmp.text = "";
            foreach (var (key, value) in chest.Content)
            {
                tmp.text += $"{key.Name}: {value}\n";
            }
        }
    }
}
