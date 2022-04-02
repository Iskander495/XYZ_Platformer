using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Components
{
    public class DoInteractComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            var interactable = go.GetComponent<InteractibleComponent>();

            interactable?.Interact();
        }
    }
}