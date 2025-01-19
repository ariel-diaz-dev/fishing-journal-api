# Fishing Journal API
.NET Core 9.0 API that supports the front of of the Fishing Journal app which lets you plan fishing outings by looking at the tides, weather forecast, plan description as well as record the actual weather conditions onsite, log species caught and map locations.

# (WIP) Feature 
- 

# (TODO) Features
- Plan a fishing trip
    - Select a date for the fishing trip
    - Enter estimated time of arrival to the fishing spot
    - Display if it's incoming or outgoing tide by the time you plan to get there
    - Displays weather forecast and tides for a location selected on a map
    - Estimates the best times for fishing
    - Recommends lures, bait, gear to be used for the targetted species
    - Shows soundings on a map
- Log a fishing report 
    - Includes map coordinates, species caught with size/weight, weather conditions, lures used, gear used (rods, reels, leader/main lines used, hooks, jigheads, swimbaits/paddletails), arrival time, departure time, kayak used 
    - Allows for a joint fishing report with multiple users
    - Allows for the reports to be private and public
    - Allows for comments and notes
- Creates a finalized fishing report on Save (includes the plan and the actual results after the day is done)
- Gear Inventory
    - Gear-tracking system where a user manages their fishing equipment.
    - Include maintenance reminders for rods, reels, or kayaks.
    - Includes kayak used
- Analytics dashboard
    - Displays calendar with fishing performance
    - Highlights the best fishing times per location
- Offline catch log tracking

# (COMPLETED) Features
- 
- 
- 


# Models
- FishingReport
    - Id
    - EstimatedArrivalTime
    - FirstHighTide
    - SecondHighTide
    - FirstLowTide
    - SecondLowTide
    - DaytimeTemperature
    - Visibility
    - EventDate
    - CreatedDate
    - DeletedDate
    - UpdatedDate
- FishSpecies
    - Id
    - Name
    - ScientificName
    - Description
    - CreatedDate
    - DeletedDate
    - UpdatedDate
- (Mapping Table) Catch
    - Id
    - FishId
    - ReportId
    - UserId
    - CreatedDate
    - DeletedDate
    - UpdatedDate
- Gear
    - Id
    - Type
    - Name
    - Description
    - CreatedDate
    - DeletedDate
    - UpdatedDate

# User Flows
### Creating a Fishing Plan
- Select date 
- Select fishing spot on the map
    - UI displays 
        - Tide information
        - Sunrise time
        - Weather Forecast
- Save plan


### Create a Fishing Report
- From the Fishing Plan UI, user can create a fishing report for that plan.
    1:1 relationship between plan and fishing report
- User logs species caught with size/weight and lure that was used for the species
- User logs gear used (rods, reels, leader/main lines used, hooks, jigheads, swimbaits/paddletails), arrival time, departure time, kayak used
    - User selects gear from personal inventory
    - gear types:
        - vessel
        - lure type
        - rod 
        - reel
        - leader
        - main line
    