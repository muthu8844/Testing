# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Run the application
dotnet run --project Dashboard/Dashboard.csproj

# Build the solution
dotnet build

# Run with a specific launch profile
dotnet run --project Dashboard/Dashboard.csproj --launch-profile http
```

The app runs on `http://localhost:5092` by default (HTTP profile). No test projects exist yet.

## Architecture

This is a **Blazor Server** (.NET 8) dashboard application with an `InteractiveServer` render mode (SignalR-based real-time updates). The solution has three projects:

- **Dashboard** – The main web app (all active UI code lives here)
- **Dashboard.Service** – Service layer (currently empty scaffolding)
- **Dashboard.DAL** – Data access layer (currently empty scaffolding)

### Component hierarchy

```
MainLayout.razor (sidebar + body shell)
├── NavMenu.razor (sidebar links)
└── @Body
    └── Dashboard.razor  [route: /]
        ├── Metrics.razor       (4 stat cards with ApexCharts sparklines)
        ├── Traffic.razor       (ApexCharts donut chart by platform/country)
        ├── RecentActivities.razor  (timeline of events)
        └── OrderStatus.razor   (orders table with search + pagination)
```

`Home.razor` is mapped to `/home` but is minimal.

### Data flow (current state)

All data is **hardcoded directly inside components** — there is no backend, database, or service injection yet. The DAL and Service projects are empty. When adding real data:

1. Add EF Core or other ORM to `Dashboard.DAL`
2. Add business logic to `Dashboard.Service`
3. Wire services into `Program.cs` via DI
4. Inject services into Razor components

### Shared data models

`Dashboard/ViewModel/Order.cs` holds:
- `Order` – Country, Percentage, GrossValue, Label, Color
- `Duration` – Month, Value (time series)
- `MetricBarChartData` – X, Y (for sparkline charts)

### Charting

Uses the `Blazor-ApexCharts` (v1.1.0) NuGet package. The `AddApexCharts()` service registration in `Program.cs` is currently **commented out** — uncomment it if chart functionality breaks.

### CSS approach

- Scoped CSS: each component can have a `.razor.css` sibling file
- Global styles: `wwwroot/app.css`
- Bootstrap (local) + Font Awesome (4.7.0 and 6.5.0) loaded in `App.razor`
- Color palette: dark blue sidebar (`#06163a`), orange accents (`#ff6b35`), light grey background (`#f4f6fb`)
