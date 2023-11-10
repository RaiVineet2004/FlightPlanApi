using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightPlan.Data;
using FlightPlan.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightPlan.Model;


namespace FlightPlan.Controllers
{

    //[Route("api/v1/FlightPlan")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        public IDatabaseAdapter _database;

        public FlightPlanController(IDatabaseAdapter database)
        {
            _database = database;
        }
        [HttpGet]
        [Route("api/v1/FlightPlan")]
        public async Task<IActionResult> FlightPlanList()
        {

            var flightPlanList = await _database.GetAllFlightPlans();
            // Throw error if No flightPlan
            if(flightPlanList.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            // Throw suceess.
            return Ok(flightPlanList);
            
        }
        [HttpGet]
        [Route("api/v1/FlightPlan/{flightPlanId}")]
        public async Task<IActionResult> GetFlightFlightPlanById(string flightPlanId)
        {
            var flightPlans = await _database.GetAllFlightPlansById(flightPlanId);

            if (flightPlans == null || flightPlans.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound); // Return a 404 Not Found response if no matching flight plans are found
            }

            // Find the flight plan with the matching ID
            var matchingFlightPlan = flightPlans.FirstOrDefault(fp => fp.FlightPlanId == flightPlanId);

            if (matchingFlightPlan == null)
            {
                return StatusCode(StatusCodes.Status404NotFound); // Return a 404 Not Found response if the flight plan with the specified ID is not found
            }

            return Ok(matchingFlightPlan);
        }

        [HttpPost]
        [Route("api/v1/FlightPlan/file")]
        public async Task<IActionResult> SaveFlightPlans(flightPlans flightPlans)
        {

            var transactionResult = await _database.FileFightPlan(flightPlans);
            switch (transactionResult)
            {
                case TransactionResult.Success:
                    return Ok();
                case TransactionResult.BadRequest:
                    return StatusCode(StatusCodes.Status400BadRequest);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
            

        }
        [HttpPut]
        [Route("api/v1/FlightPlan")]
        public async Task<IActionResult> UpdateFlightPlan(flightPlans flightPlans)
        {
            var transactionResult = await _database.UpdateFlightPlan(flightPlans.FlightPlanId, flightPlans);
            switch (transactionResult)
            {
                case TransactionResult.Success:
                    return Ok();
                case TransactionResult.BadRequest:
                    return StatusCode(StatusCodes.Status400BadRequest);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete]
        [Route("api/v1/FlightPlan/{flightPlanId}")]
        public async Task<IActionResult> DeleteFlightPlan (string flightPlanId)
        {
            var result = await _database.DeleteFlightPlan(flightPlanId);
            if (result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

        }
        
        [HttpGet]
        [Route("api/v1/FlightPlan/airport/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanRoute(string flightPlanId)
        {
            var flightPlansList = await _database.GetAllFlightPlansById(flightPlanId);

            if (flightPlansList == null || flightPlansList.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            // Assuming you want the route of the first flight plan in the list
            var firstFlightPlan = flightPlansList[0];

            return Ok(firstFlightPlan.Route);
        }
        [HttpGet]
        [Route("api/v1/FlightPlan/airport/enroute/{flightPlanId}")]
        public async Task<IActionResult> GetFlightPlanTimeEnroute(string flightPlanId)
        {
            var flightPlansList = await _database.GetAllFlightPlansById(flightPlanId);

            if (flightPlansList == null || flightPlansList.Count == 0)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            // Assuming you want the route of the first flight plan in the list
            var firstFlightPlan = flightPlansList[0];

            // Calculate estimated time enroute (assuming ArrivalTime and DepartureTime are DateTime properties)
            TimeSpan estimatedTimeEnroute = firstFlightPlan.ArrivalTime - firstFlightPlan.DepartureTime;

            return Ok(estimatedTimeEnroute);
        }

    }
}
