using System;
using System.Collections.Generic;
using SonicFramework.CharacterFlags;
using UnityEngine;

namespace SonicFramework
{
    public class PlayerFlags : PlayerComponent
    {
        public List<Flag> List;

        private void Awake()
        {
            List = new List<Flag>();
        }

        public void Add(Flag flag)
        {
            if (!List.Contains(flag))
            {
                List.Add(flag);
            }
        }

        public void Remove(Flag flag)
        {
            if (List.Contains(flag))
            {
                List.Remove(flag);
            } 
        }

        public bool Check(Flag flag)
        {
            return List.Contains(flag);
        }
    }
}