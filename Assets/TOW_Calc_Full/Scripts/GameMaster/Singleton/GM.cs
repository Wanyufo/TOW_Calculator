using System;
using UnityEngine;

namespace GameMaster
{
    /// <summary>
    ///     Singleton Game Master, handles access to Managers
    /// </summary>
    public class GM : MonoBehaviour
    {
        [NonSerialized] public AudioManager AudioManager;
        [NonSerialized] public DebugManager DebugManager;
        [NonSerialized] public TagManager TagManager;
        

        /// <summary>
        ///     Local Awake Function, Same as MonoBehaviour Awake, but is only called on the unique Singleton
        /// </summary>
        private void GmAwake()
        {
        }


        /// <summary>
        ///     Find all Managers and fill the Manager references
        ///     Needs to be Updated Manually when new Manager Types are added
        /// </summary>
        private void GatherManagers()
        {
            try
            {
                TagManager = FindObjectOfType<TagManager>();
                AudioManager = FindObjectOfType<AudioManager>();
                DebugManager = FindObjectOfType<DebugManager>();
            }
            catch (Exception e)
            {
                Debug.LogError("Error in GM GatherManagers: " + e.Message);
                throw;
            }
        }

        // ############## SINGLETON INITIALISATION ###################
        public static GM I { get; private set; }

        private void Awake()
        {
            if (I == null)
            {
                I = this;
                GatherManagers();
                GmAwake();
            }
            else
            {
                Destroy(this);
                Debug.LogError("A second GM existed, has been destroyed");
            }
        }
    }
}