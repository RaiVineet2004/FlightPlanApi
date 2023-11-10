using System.Collections.Generic;
using System.Threading.Tasks;
using FlightPlan.Model;

namespace FlightPlan.Data
{
	public interface IDatabaseAdapter
	{
		Task<List<flightPlans>> GetAllFlightPlans(); // Get all 

		Task<List<flightPlans>> GetAllFlightPlansById(string flightPlanId); // Get Flight plan by ID

		Task<TransactionResult> FileFightPlan(flightPlans flight); // To Post need a Flight

		Task<TransactionResult> UpdateFlightPlan(string FlightPlanId, flightPlans flight);  // to update

		Task<bool> DeleteFlightPlan(string FlightPlanId); // To Delete
	}
}

