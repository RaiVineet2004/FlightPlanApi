
using System;
using System.Text.Json.Serialization;

namespace FlightPlan.Model
{
	public class flightPlans
	{
        [JsonPropertyName("flight_plan_id")]
        public string? FlightPlanId { get; set; }
        [JsonPropertyName("aircraft_identification")]
        public string? AircraftIdentification { get; set; }
        [JsonPropertyName("airspeed")]
        public int Airspeed { get; set; }
        [JsonPropertyName("altitude")]
        public int Altitude { get; set; }
        [JsonPropertyName("aircraft_type")]
        public string? AircraftType { get; set; }
        [JsonPropertyName("arrival_airport")]
        public string? ArrivalAirport { get; set; }
        [JsonPropertyName("flight_type")]
        public string? FlightType { get; set; }
        [JsonPropertyName("departure_time")]
        public DateTime DepartureTime { get; set; }
        [JsonPropertyName("estimated_arrival_time")]
        public DateTime ArrivalTime { get; set; }
        [JsonPropertyName("fuel_hours")]
        public int FuelHours { get; set; }
        [JsonPropertyName("fuel_minutes")]
        public int FuelMinutes { get; set; }
        [JsonPropertyName("number_onboard")]
        public int NumberOnboard { get; set; }
        [JsonPropertyName("route")]
        public string? Route { get; set; }
        [JsonPropertyName("remarks")]
        public string? Remarks { get; set; }

    }
}

