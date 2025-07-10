using System;
using CarSpot.Domain.Common;

namespace CarSpot.Domain.Events
{
    public class VehicleCreatedEvent : IDomainEvent
    {
        public Guid VehicleId { get; }
        public Guid UserId { get; }

        public VehicleCreatedEvent(Guid vehicleId, Guid userId)
        {
            VehicleId = vehicleId;
            UserId = userId;
        }
    }

}