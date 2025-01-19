# Fishing Journal API
.NET Core 9.0 API that supports the front of of the Fishing Journal app which lets you plan fishing outings by looking at the tides, weather forecast, plan description as well as record the actual weather conditions onsite, log species caught and map locations.

# (WIP) Feature 
- 

# (TODO) Features
- Plan a fishing trip
    - Select a date for the fishing trip
    - Select a place / area to fish
    - Displays weather forecast by hour
    - Displays tide information
    - Display water temperature
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
    - FirstHighTide
    - SecondHighTide
    - FirstLowTide
    - SecondLowTide
    - DaytimeTemperature
    - 
    - Visibility
    - EventDate
    - CreatedDate
    - DeletedDate
    - UpdatedDate
- FishSpecies
    - FishId
    - Name
    - ScientificName
    - Description
    - CreatedDate
    - DeletedDate
    - UpdatedDate
- (Mapping Table) Catch
    - FishId
    - ReportId
    - UserId
    - CreatedDate
    - DeletedDate
    - UpdatedDate
- Gear
- 
