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

- **[DONE] Location**
    - Id: `Int` (Primary Key)
    - Order: `Int`
    - Name: `string` (Required, MaxLength: 200)
    - Latitude: `decimal`
    - Longitude: `decimal`
    - Description: `string` (MaxLength: 1000)
    - CreatedDate: `DateTime`

- **[DONE] FishSpecies**
    - Id: `Int` (Primary Key)
    - Order: `Int`
    - Name: `string` (Required, MaxLength: 100)
    - ScientificName: `string` (MaxLength: 150)
    - Description: `string` (MaxLength: 1000)
    - CreatedDate: `DateTime`

- **[DONE] FishingReport**
    - Id: `Guid` (Primary Key)
    - AccountId: `Guid` (Foreign Key)
    - LocationId: `Int` (Foreign Key)
    - ArrivalTime: `DateTime?`
    - DepartureTime: `DateTime?`
    - FirstHighTide: `DateTime?`
    - SecondHighTide: `DateTime?`
    - FirstLowTide: `DateTime?`
    - SecondLowTide: `DateTime?`
    - DaytimeTemperature: `decimal?`
    - WaterTemperature: `decimal?`
    - WindSpeedInMilesPerHour: `Int?`
    - WindDirection: `string?` (MaxLength: 10)
    - WeatherConditions: (enum: Windy, Cloudy, Sunny, Hot, Very Hot, Cold, Very Cold, Rainy, Windy, Foggy, Other)
    - Notes: `string?` (MaxLength: 2000)
    - TripDate: `DateTime?`
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime?`

- **Landings**
    - Id: `Guid` (Primary Key)
    - AccountId: `Guid` (Foreign Key)
    - FishSpeciesId: `Int` (Foreign Key)
    - FishingReportId: `Guid` (Foreign Key)
    - LengthInInches: `decimal?`
    - LureUsed: `Guid` (Foreign Key for TackleId)
    - RodUsed: `Guid` (Foreign Key for TackleId)
    - ReelUsed: `Guid` (Foreign Key for TackleId)
    - MainLineTestInPounds: `Int?`
    - LeaderLineTestInPounds: `Int?`
    - TimeOfCatch: `DateTime?`
    - Released: `bool` (Default: true)
    - CreatedDate: `DateTime`
    - DeletedDate: `DateTime?`
    - UpdatedDate: `DateTime?`

- **ReportTackle**
    - Id: `Guid` (Primary Key)
    - FishingReportId: `Guid` (Foreign Key)
    - TackleId: `Guid` (Foreign Key)
    - CreatedDate: `DateTime`
