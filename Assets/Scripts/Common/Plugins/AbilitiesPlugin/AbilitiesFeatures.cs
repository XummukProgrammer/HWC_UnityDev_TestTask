using System;
using System.Collections.Generic;

public class AbilitiesFeatures
{
    private List<IAbilityFeature> _features = new();

    public IEnumerator<IAbilityFeature> Features => _features.GetEnumerator();

    public void Add<T>() where T : IAbilityFeature
    {
        var feature = Activator.CreateInstance<T>();
        feature.OnInit();
        _features.Add(feature);
    }

    public IAbilityFeature Get(string id)
    {
        foreach (var feature in _features)
        {
            if (feature.Id == id)
            {
                return feature;
            }
        }
        return null;
    }
}
