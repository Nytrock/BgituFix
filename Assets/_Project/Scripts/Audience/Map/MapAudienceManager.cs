using System;
using System.Collections.Generic;
using UnityEngine;

public class MapAudienceManager : MonoBehaviour {
    [SerializeField] private MapAudiencePool _audiencePool;
    [SerializeField] private float _cameraSizeOffset;

    private readonly List<MapAudience> _audiences = new();
    private MapAudience _nowAudience;

    public event Action<bool> StateChanged;

    public void GenerateAudiences(MapData mapData) {
        foreach (var audienceData in mapData.AudienceDatas) {
            MapAudience audience = _audiencePool.GetObject();
            audience.ChangeState(false);
            audience.Setup(mapData, audienceData);
            _audiences.Add(audience);
        }
    }

    public void OpenAudience(AudienceData audience) {
        foreach (var mapAudience in _audiences)
            if (mapAudience.Id == audience.Id)
                UpdateAudience(mapAudience);
        ChangeState(true);
    }

    private void UpdateAudience(MapAudience audience) {
        if (_nowAudience != null)
            _nowAudience.ChangeState(false);
        _nowAudience = audience;
        _nowAudience.ChangeState(true);
        UpdateCameraSize();
    }

    private void UpdateCameraSize() {
        float newSize = _nowAudience.CameraSize;
        Camera.main.orthographicSize = newSize + newSize / _cameraSizeOffset;
    }

    public void ChangeState(bool newState) {
        gameObject.SetActive(newState);
        StateChanged?.Invoke(newState);
    }
}
