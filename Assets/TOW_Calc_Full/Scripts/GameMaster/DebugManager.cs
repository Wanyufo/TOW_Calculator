using UnityEngine;

namespace GameMaster
{
    /// <summary>
    ///     Handles the activity of debug components and behaviours
    /// </summary>
    public class DebugManager : MonoBehaviour, IManager
    {
        [SerializeField] private bool debugActive;
        [SerializeField] private GameObject[] debugObjects;
        [SerializeField] private Behaviour[] debugBehaviours;

        // Start is called before the first frame update
        private void Start()
        {
            SetDebugActive(debugActive);
        }

        public void ToggleDebugMode()
        {
            debugActive = !debugActive;
            SetDebugActive(debugActive);
        }


        public void SetDebugActive(bool value)
        {
            debugActive = value;
            foreach (var debugObject in debugObjects) debugObject.SetActive(value);

            foreach (var debugComponent in debugBehaviours) debugComponent.enabled = value;
        }
    }
}