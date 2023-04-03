using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterStat 
{
    public int BaseValue;

    public virtual int Value
    {
        get
        {
            if (isDirty|| BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalcFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    protected bool isDirty = true;
    protected int _value;
    protected int lastBaseValue = int.MinValue;

    protected readonly List<StatModifier> statModifiers;
    protected readonly ReadOnlyCollection<StatModifier> StatModifiers;

    public CharacterStat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public CharacterStat(int baseValue) : this()
    {
        BaseValue = baseValue;
    }
    
    public virtual void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if(statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        for (int i = statModifiers.Count-1; i >=0; i--)
        {
            if(statModifiers[i].Source==source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    protected virtual int CalcFinalValue()
    {
        int finalValue = BaseValue;

        for (int i=0; i < statModifiers.Count; i++)
        {
            finalValue += statModifiers[i].Value; 
        }

        return finalValue;
    }

    
}
