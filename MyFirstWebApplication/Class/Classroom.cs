using System;
using System.Collections.Generic;

namespace MyFirstWebApplication.Class
{
    public class Classroom
    {
        private readonly int _id;
        private string _roomName;
        private double _size;
        private int _capacity;

        public Classroom(int id, string roomName, double size, int capacity, bool hasCynapSystem)
        {
            if (id <= 0) throw new ArgumentException("ID must be positive.", nameof(id));
            if (string.IsNullOrWhiteSpace(roomName)) throw new ArgumentException("Room name cannot be empty.", nameof(roomName));
            if (size <= 0) throw new ArgumentException("Size must be positive.", nameof(size));
            if (capacity <= 0) throw new ArgumentException("Capacity must be positive.", nameof(capacity));

            _id = id;
            _roomName = roomName;
            _size = size;
            _capacity = capacity;
            HasCynapSystem = hasCynapSystem;
        }

        public int Id => _id;
        public string RoomName
        {
            get => _roomName;
            set => _roomName = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Room name cannot be empty.", nameof(value)) : value;
        }
        public double Size
        {
            get => _size;
            set => _size = value <= 0 ? throw new ArgumentException("Size must be positive.", nameof(value)) : value;
        }
        public int Capacity
        {
            get => _capacity;
            set => _capacity = value <= 0 ? throw new ArgumentException("Capacity must be positive.", nameof(value)) : value;
        }
        public bool HasCynapSystem { get; set; }
    }
}