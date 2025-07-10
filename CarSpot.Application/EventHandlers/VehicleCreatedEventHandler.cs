using CarSpot.Application.DTOs;
using CarSpot.Application.Interfaces;
using CarSpot.Domain.Common;
using CarSpot.Domain.Events;

public class VehicleCreatedEventHandler : IDomainEventHandler<VehicleCreatedEvent>
{
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleCreatedEventHandler(IEmailService emailService, IUserRepository userRepository, IVehicleRepository vehicleRepository)
    {
        _emailService = emailService;
        _userRepository = userRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task HandleAsync(VehicleCreatedEvent domainEvent)
    {
        Console.WriteLine($"VehicleCreatedEventHandler: Processing event for Vehicle {domainEvent.VehicleId}, User {domainEvent.UserId}");
        
        var user = await _userRepository.GetByIdAsync(domainEvent.UserId);
        var vehicle = await _vehicleRepository.GetByIdAsync(domainEvent.VehicleId);

        if (user != null && vehicle != null)
        {
            Console.WriteLine($"VehicleCreatedEventHandler: Sending email to {user.Email} for vehicle {vehicle.Title}");
            
            var vehicleEmailData = new VehicleCreatedEmailDto(
                user.FullName,
                user.Email,
                vehicle.Title,
                vehicle.VIN,
                vehicle.Make?.Name ?? "N/A",
                vehicle.Model?.Name ?? "N/A",
                vehicle.Year
            );

            await _emailService.SendEmailAsync(
                user.Email,
                "Vehículo creado exitosamente",
                EmailTemplateType.VehicleCreated,
                vehicleEmailData,
                "Notifications"
            );
        }
        else
        {
            Console.WriteLine($"VehicleCreatedEventHandler: User or Vehicle not found. User: {user != null}, Vehicle: {vehicle != null}");
        }
    }
}
