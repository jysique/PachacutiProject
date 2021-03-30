using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarBoss : Subordinate
{

    [SerializeField] private int experience;
    [SerializeField] private string strategyType;

    private TYPESTRAT type;
    public TYPESTRAT Type
    {
        get { return (MilitarBoss.TYPESTRAT)System.Enum.Parse(typeof(MilitarBoss.TYPESTRAT), StrategyType); }
    }

    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

    public string StrategyType
    {
        get { return strategyType; }
        set { strategyType = value; }
    }

    public enum TYPESTRAT
    {
        AGGRESSIVE,
        TERRAIN_MASTER,
        DEFENSIVE,
        SACRED_WARRIOR,
        SIEGE_EXPERT
    }

}
