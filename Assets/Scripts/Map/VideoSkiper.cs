using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace Game.Map
{
    public class VideoSkiper : MonoBehaviour
    {
        [SerializeField] private List<VideoPlayer> _videoPlayer;
        [SerializeField] private float _readyTime;
        [SerializeField, ReadOnly] private bool _isReady;
        [SerializeField] private GameObject _readyShow;

        private void Start() { Invoke(nameof(Ready), _readyTime); }

        private void Ready()
        {
            _isReady = true;
            _readyShow.SetActive(true);
        }

        public void Skip()
        {
            if (!_isReady) return;

            _videoPlayer.ForEach(x => x.Stop());
        }
    }
}