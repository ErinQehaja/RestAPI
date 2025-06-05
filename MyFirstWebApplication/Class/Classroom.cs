using System;
using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApplication.Class
{
    public class Classroom
    {
        public int Id { get; set; } // Changed to public set for EF
        public string RoomName { get; set; } = null!; // EF requires non-nullable for required fields
        public double Size { get; set; }
        public int Capacity { get; set; }
        public bool HasCynapSystem { get; set; }

        public int SchoolId { get; set; } // Foreign key to School
        public School School { get; set; } // Navigation property for EF

        public Classroom() { } // Parameterless constructor for EF

        public Classroom(int id, string roomName, double size, int capacity, bool hasCynapSystem)
        {
            if (id <= 0) throw new ArgumentException("ID must be positive.", nameof(id));
            if (string.IsNullOrWhiteSpace(roomName)) throw new ArgumentException("Room name cannot be empty.", nameof(roomName));
            if (size <= 0) throw new ArgumentException("Size must be positive.", nameof(size));
            if (capacity <= 0) throw new ArgumentException("Capacity must be positive.", nameof(capacity));

            Id = id;
            RoomName = roomName;
            Size = size;
            Capacity = capacity;
            HasCynapSystem = hasCynapSystem;
        }
    }
}