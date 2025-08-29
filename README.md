# Fishing Journal API
.NET 8 API that supports the front end of the Fishing Journal app which lets you save a fishing report with the actual weather conditions onsite, log species caught, map location, tackle used, notes, etc.

# Features
- Log a fishing report 
    - Includes map coordinates, species caught with size/weight, weather conditions, lures used, gear used (rods, reels, leader/main lines used, hooks, jigheads, swimbaits/paddletails), arrival time, departure time, kayak used 
    - Allows for a joint fishing report with multiple users
    - Allows for the reports to be private and public
    - Allows for comments and notes
- Creates a finalized fishing report on Save
- Gear Inventory
    - Gear-tracking system where a user manages their fishing equipment.
    - Include maintenance reminders for rods, reels, or kayaks.
    - Includes kayak used
- Analytics dashboard
    - Displays calendar with fishing performance
    - Highlights the best fishing times per location
- Offline catch log tracking

# Models
- **[DONE] Account**
    - Id: `Guid` (Primary Key)
    - Email: `string` (Required, MaxLength: 255)
    - FirstName: `string` (Required, MaxLength: 100)
    - LastName: `string` (Required, MaxLength: 100)
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **[DONE] Tackle**
    - Id: `Guid` (Primary Key)
    - AccountId: `Guid` (Foreign Key)
    - Type: `Type` (Required, enum: Rod, Reel, Line, Lure, Terminal, Vessel, Other)
    - Name: `string` (Required, MaxLength: 200)
    - Description: `string` (MaxLength: 1000)
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime`

- **Location**
    - Id: `Int` (Primary Key)
    - Name: `string` (Required, MaxLength: 200)
    - Latitude: `decimal`
    - Longitude: `decimal`
    - Description: `string` (MaxLength: 1000)
    - CreatedDate: `DateTime`

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

- **FishingReport**
    - Id: `Guid` (Primary Key)
    - AccountId: `Guid` (Foreign Key)
    - LocationId: `Guid` (Foreign Key)
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

- **ReportTackle** (Mapping Table)
    - Id: `Guid` (Primary Key)
    - FishingReportId: `Guid` (Foreign Key)
    - TackleId: `Guid` (Foreign Key)
    - CreatedDate: `DateTime`

