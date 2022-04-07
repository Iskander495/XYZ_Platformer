using Components;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkStore : MonoBehaviour
{
    private List<Perk> _perks;

    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();

        _perks = _session.GetValue<List<Perk>>("Perks");
    }

    public void AddPerk(Perk perk)
    {
        if (!_perks.Contains(perk))
            _perks.Add(perk);
    }

    public void RemovePerk(Perk perk)
    {
        if (_perks.Contains(perk))
            _perks.Remove(perk);
    }

    public bool PresentPerk(Perk perk)
    {
        return (_session != null && _perks != null) 
            ? _perks.Contains(perk) 
            : false;
    }
}
