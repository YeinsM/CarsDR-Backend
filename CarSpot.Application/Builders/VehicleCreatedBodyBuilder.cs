using CarSpot.Application.DTOs;

public class VehicleCreatedBodyBuilder : IEmailBodyBuilder<VehicleCreatedEmailDto>
{
    public string Build(VehicleCreatedEmailDto vehicleData)
    {
        return $@"
            <p>¡Hola {vehicleData.FullName}!</p>
            <p>Tu vehículo ha sido creado exitosamente en CarSpot.</p>
            <div style='background-color: #f8f9fa; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                <h3 style='color: #0D3A52; margin-top: 0;'>Detalles del Vehículo:</h3>
                <p><strong>Título:</strong> {vehicleData.VehicleTitle}</p>
                <p><strong>VIN:</strong> {vehicleData.VIN}</p>
                <p><strong>Marca:</strong> {vehicleData.Make}</p>
                <p><strong>Modelo:</strong> {vehicleData.Model}</p>
                <p><strong>Año:</strong> {vehicleData.Year}</p>
            </div>
            <p>Ya puedes empezar a gestionar tu vehículo desde tu panel de usuario.</p>
            <p>¡Gracias por confiar en CarSpot!</p>";
    }
}