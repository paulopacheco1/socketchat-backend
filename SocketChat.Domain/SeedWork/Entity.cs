using System;

namespace SocketChat.Domain.SeedWork
{
    public abstract class Entity
    {
        protected Entity() {}
        
        public int Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public void SetCreated()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public void SetUpdated()
        {
            UpdatedAt = DateTime.Now;
        }
    }
}