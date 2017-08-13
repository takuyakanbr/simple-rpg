using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    // contains data about an entity (objects / NPC)
    public class Entity
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string Description;

        // run when interactable is being added to map;
        // entity will not be added if this returns false
        public Func<GameState, Player, bool> OnCreate;

        // run when player interacts with this entity
        public Action<GameState, Player> OnInteract;

        public Entity(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}
