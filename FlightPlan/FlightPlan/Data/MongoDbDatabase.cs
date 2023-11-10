using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using FlightPlan.Model;
using System.Text.Json.Serialization;

namespace FlightPlan.Data
{
    public class MongoDbDatabase : IDatabaseAdapter
    {
		// Get all the flights
        public async Task<List<flightPlans>>  GetAllFlightPlans()
        {

            var collection = GetCollection("Vineetrai", "FlightPlan"); // Getting the data collection from MongoDb Api
            var documents = collection.Find(_ => true).ToListAsync();

            var flightPlanList = new List<flightPlans>();
            if (documents == null) return flightPlanList;

			foreach ( var document in await documents) 
			{
				flightPlanList.Add(ConvertBsonToFlightPlan(document));
			}
			return flightPlanList;
        }
		// Get all the Flight By Id
        public async Task<List<flightPlans>> GetAllFlightPlansById(string flightPlanId)
        {
            var collection = GetCollection("Vineetrai", "FlightPlan"); // Getting the data collection from MongoDB API
            var flightPlanCursor = await collection.FindAsync(
                Builders<BsonDocument>.Filter.Eq("flight_plan_id", flightPlanId));

            var documents = await flightPlanCursor.ToListAsync();

            if (documents.Count == 0)
            {
                return new List<flightPlans>(); // Return an empty list if no documents are found
				
            }

            // Convert all documents to flight plans and return them in a list
            var flightPlanList = documents.Select(ConvertBsonToFlightPlan).ToList();
            return flightPlanList;
        }
		// Put
		public async Task<TransactionResult> FileFightPlan( flightPlans flightPlan)
		{
			var collection = GetCollection("Vineetrai", "FlightPlan");
			var document = new BsonDocument
			{
				{"flight_plan_id", Guid.NewGuid().ToString("N")},
				{"altitude" , flightPlan.Altitude},
				{"airspeed" , flightPlan.Airspeed },
				{"aircraft_identification", flightPlan.AircraftIdentification },
                {"aircraft_type" , flightPlan.AircraftType },
                {"arrival_airport" , flightPlan.ArrivalAirport},
                {"flight_type" , flightPlan.FlightType},
                {"departure_time" , flightPlan.DepartureTime},
                {"estimated_arrival_time" , flightPlan.ArrivalTime},
                {"fuel_hours" , flightPlan.FuelHours},
                {"fuel_minutes" , flightPlan.FuelMinutes},
                {"number_onboard" , flightPlan.NumberOnboard},
                {"route" , flightPlan.Route},
                {"remarks" , flightPlan.Remarks}

            };


			await collection.InsertOneAsync(document);

			try
			{
				await collection.InsertOneAsync(document);
				
				if (document["_id"].IsObjectId)
				{
					return TransactionResult.Success;
				}
				return TransactionResult.BadRequest;
			}
			catch 
			{
				return TransactionResult.ServerError;
			}

			
		}
		// post
		public async Task<TransactionResult> UpdateFlightPlan ( string flightPlanId , flightPlans flightPlan)
		{
            var collection = GetCollection("Vineetrai", "FlightPlan");
			var Filter = Builders<BsonDocument>.Filter.Eq("flight_plan_id", flightPlanId);
			var update = Builders<BsonDocument>.Update
				.Set("altitude", flightPlan.Altitude)
				.Set("airspeed", flightPlan.Airspeed)
				.Set("aircraft_identification", flightPlan.AircraftIdentification)
				.Set("aircraft_type", flightPlan.AircraftType)
				.Set("arrival_airport", flightPlan.ArrivalAirport)
                .Set("estimated_arrival_time", flightPlan.ArrivalTime)
                .Set("departure_time", flightPlan.DepartureTime)
				.Set("flight_type", flightPlan.FlightType)
                .Set("fuel_hours", flightPlan.FuelHours)
                .Set("fuel_minutes", flightPlan.FuelMinutes)
				.Set("number_onboard", flightPlan.NumberOnboard)
				.Set("route", flightPlan.Route)
				.Set("remarks", flightPlan.Remarks);


			var result = await collection.UpdateOneAsync(Filter, update);
			if(result.MatchedCount == 0)
			{
				return TransactionResult.NotFound;
			}
			if(result.MatchedCount > 0)
			{
				return TransactionResult.Success;
			}
			return TransactionResult.ServerError;
        }
		//Delete
        public async Task<bool> DeleteFlightPlan( string flightPlanId)
        {
			var collection = GetCollection("Vineetrai", "FlightPlan");
			var result = await collection.DeleteOneAsync(
				Builders<BsonDocument>.Filter.Eq("flight_plan_id", flightPlanId));

			return result.DeletedCount > 0;
        }


        private IMongoCollection<BsonDocument> GetCollection(string DatabaseName, string collectionName)
		{
			var client = new MongoClient();
			var database = client.GetDatabase(DatabaseName);  //	Getting the Database Name.
			var collection = database.GetCollection<BsonDocument>(collectionName); // getting the database Collection.
			return collection;
		}

		private flightPlans ConvertBsonToFlightPlan (BsonDocument document)
		{
			if (document == null) return null;

			return new flightPlans
			{
				FlightPlanId = document["_id"].AsString,
				AircraftIdentification = document["aircraft_identification"].AsString,
                Airspeed = document["airspeed"].AsInt32,
                Altitude = document["altitude"].AsInt32,
				AircraftType = document["aircraft_type"].AsString,
				ArrivalAirport = document["arrival_airport"].AsString,
				FlightType = document["flight_type"].AsString,
				DepartureTime = document["departure_time"].AsBsonDateTime.ToUniversalTime(),
				ArrivalTime = document["estimated_arrival_time"].AsBsonDateTime.ToUniversalTime(),
                Route = document["route"].AsString,
				Remarks = document["remarks"].AsString,
				FuelHours = document["fuel_hours"].AsInt32,
				FuelMinutes = document["fuel_minutes"].AsInt32,
				NumberOnboard = document["number_onboard"].AsInt32
			};
		}
	}
}
