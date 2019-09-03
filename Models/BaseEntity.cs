using System;

namespace SimpleBeep.Models
{
    public abstract class BaseEntity
    {
        //public int Id { get; set; }
        public Guid Id { get; set; }
    }
}