# Fishing Journal API
.NET 8 API that supports the front of of the Fishing Journal app which lets you plan fishing outings by looking at the tides, weather forecast, plan description as well as record the actual weather conditions onsite, log species caught and map locations.

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
- **Account**
    - Id: `Guid` (Primary Key)
    - Email: `string` (Required, MaxLength: 255)
    - FirstName: `string` (Required, MaxLength: 100)
    - LastName: `string` (Required, MaxLength: 100)
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **FishingReport**
    - Id: `Guid` (Primary Key)
    - AccountId: `Guid` (Foreign Key)
    - PlanId: `Guid?` (Foreign Key, nullable for standalone reports)
    - LocationLatitude: `decimal`
    - LocationLongitude: `decimal`
    - LocationName: `string` (MaxLength: 200)
    - EstimatedArrivalTime: `DateTime?`
    - ActualArrivalTime: `DateTime?`
    - ActualDepartureTime: `DateTime?`
    - FirstHighTide: `DateTime?`
    - SecondHighTide: `DateTime?`
    - FirstLowTide: `DateTime?`
    - SecondLowTide: `DateTime?`
    - DaytimeTemperature: `decimal?`
    - WaterTemperature: `decimal?`
    - Visibility: `string` (MaxLength: 100)
    - WindSpeed: `decimal?`
    - WindDirection: `string` (MaxLength: 10)
    - WeatherConditions: `string` (MaxLength: 500)
    - Notes: `string` (MaxLength: 2000)
    - IsPublic: `bool` (Default: false)
    - EventDate: `DateTime`
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **FishSpecies**
    - Id: `Guid` (Primary Key)
    - Name: `string` (Required, MaxLength: 100)
    - ScientificName: `string` (MaxLength: 150)
    - Family: `string` (MaxLength: 100)
    - Description: `string` (MaxLength: 1000)
    - AverageLength: `decimal?`
    - AverageWeight: `decimal?`
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **Catches** (Mapping Table)
    - Id: `Guid` (Primary Key)
    - FishSpeciesId: `Guid` (Foreign Key)
    - FishingReportId: `Guid` (Foreign Key)
    - AccountId: `Guid` (Foreign Key)
    - Length: `decimal?`
    - Weight: `decimal?`
    - LureUsed: `string` (MaxLength: 200)
    - TimeOfCatch: `DateTime?`
    - Released: `bool` (Default: true)
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **Gear**
    - Id: `Guid` (Primary Key)
    - AccountId: `Guid` (Foreign Key)
    - Type: `GearType` (enum: Rod, Reel, Line, Lure, Tackle, Vessel)
    - Name: `string` (Required, MaxLength: 200)
    - Brand: `string` (MaxLength: 100)
    - Model: `string` (MaxLength: 100)
    - Description: `string` (MaxLength: 1000)
    - PurchaseDate: `DateTime?`
    - PurchasePrice: `decimal?`
    - Condition: `string` (MaxLength: 50)
    - LastMaintenanceDate: `DateTime?`
    - NextMaintenanceDate: `DateTime?`
    - IsActive: `bool` (Default: true)
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **FishingPlan**
    - Id: `Guid` (Primary Key)
    - AccountId: `Guid` (Foreign Key)
    - LocationLatitude: `decimal`
    - LocationLongitude: `decimal`
    - LocationName: `string` (MaxLength: 200)
    - PlannedDate: `DateTime`
    - PlannedArrivalTime: `DateTime?`
    - PlannedDepartureTime: `DateTime?`
    - TargetSpecies: `string` (MaxLength: 500)
    - Notes: `string` (MaxLength: 2000)
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **ReportGear** (Mapping Table)
    - Id: `Guid` (Primary Key)
    - FishingReportId: `Guid` (Foreign Key)
    - GearId: `Guid` (Foreign Key)
    - CreatedDate: `DateTime`

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
    
