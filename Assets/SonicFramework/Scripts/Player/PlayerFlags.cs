using System;
using System.Collections.Generic;
using SonicFramework.CharacterFlags;
using UnityEngine;

namespace SonicFramework
{
    public class PlayerFlags : PlayerComponent
    {
        public List<Flag> FlagsList;

        private void Awake()
        {
            FlagsList = new List<Flag>();
        }

        public void Add(Flag flag)
        {
            if (!FlagsList.Contains(flag))
            {
                FlagsList.Add(flag);
            }
        }

        public void Remove(Flag flag)
        {
            if (FlagsList.Contains(flag))
            {
                FlagsList.Remove(flag);
            } 
        }

        public bool Check(Flag flag)
        {
            return FlagsList.Contains(flag);
        }
    }
}