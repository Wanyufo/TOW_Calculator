using UnityEngine;

namespace GameMaster
{
    /// <summary>
    ///     Manages a pool of spatial and a single non-spatial sound source.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour, IManager
    {
        // Create Pool in start from prefab rather than precreating it
        [SerializeField] private AudioSource[] spatialAudioSourcePool;

        private int _nextSpatialAudioSource;
        private AudioSource _nonSpatialAudioSource;
        private int _spatialAudioSourceCount;

        private void Start()
        {
            _nonSpatialAudioSource = GetComponent<AudioSource>();
            _spatialAudioSourceCount = spatialAudioSourcePool.Length - 1;
            if (_spatialAudioSourceCount <= 0)
            {
                Debug.LogError("AudioManager: No spatial audio sources in pool.");
            }
        }

        public void PlayNonSpatial(AudioClip clip)
        {
            if (clip is not null) _nonSpatialAudioSource.PlayOneShot(clip);
        }

        public void PlaySpatial(AudioClip clip, Vector3 position)
        {
            if (clip is null) return;
            var source = spatialAudioSourcePool[_nextSpatialAudioSource];
            source.transform.position = position;
            source.PlayOneShot(clip);
            _nextSpatialAudioSource = (_nextSpatialAudioSource + 1) % _spatialAudioSourceCount;
        }
    }
}