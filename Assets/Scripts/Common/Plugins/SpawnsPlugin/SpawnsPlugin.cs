using System.Collections.Generic;
using UnityEngine;

public class SpawnsPlugin : Plugin
{
    public override string Name => "Spawns";
    public override string Version => "1.0.0";
    public override string Author => "Xummuk97";

    private SpawnsContainerView _containerView;
    private Dictionary<int, bool> _freePoints = new();

    public override void OnLoad()
    {
        base.OnLoad();

        _containerView = GameObject.FindAnyObjectByType<SpawnsContainerView>();
        if (_containerView == null)
        {
            return;
        }
        
        for (int i = 0; i < _containerView.Count; i++)
        {
            _freePoints[i] = true;
        }
    }

    public override void OnUnload()
    {
        base.OnUnload();
    }

    public Vector3 Use()
    {
        if (_containerView == null)
        {
            return Vector3.zero;
        }

        foreach (var freePoint in _freePoints)
        {
            if (freePoint.Value)
            {
                _freePoints[freePoint.Key] = false;

                var entity = _containerView.Get(freePoint.Key);
                if (entity != null)
                {
                    return entity.transform.position;
                }
            }
        }    

        return Vector3.zero;
    }
}
