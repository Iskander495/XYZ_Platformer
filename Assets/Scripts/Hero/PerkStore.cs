using Components;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkStore : MonoBehaviour
{
    private List<Perk> _perks => _session.Data.Perks;

    private GameSession _session;

    private void Start()
    {
        _session = FindObjectOfType<GameSession>();
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
        return _perks.Contains(perk);
    }
}
