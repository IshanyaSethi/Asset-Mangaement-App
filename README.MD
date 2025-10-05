# Asset Management System

A comprehensive web application for managing company assets, employees, and assignments built with ASP.NET Core Blazor Server.

## 🚀 Features

### Core Functionality
- **Employee Management**: Add, edit, delete, and search employees
- **Asset Inventory**: Track all company assets with detailed information
- **Assignment System**: Assign assets to employees and track history
- **Dashboard**: Real-time statistics and overview
- **Reports**: Export data to CSV and generate assignment reports

### Key Capabilities
- Real-time search and filtering
- Status tracking (Available, Assigned, Under Repair, Retired)
- Warranty expiration alerts
- Assignment history tracking
- Data validation and error handling
- Responsive Bootstrap UI

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core Blazor Server (.NET 8/9)
- **Database**: SQLite with Entity Framework Core
- **ORM**: Entity Framework Core (for CRUD operations)
- **Micro-ORM**: Dapper (for high-performance read queries)
- **UI**: Bootstrap 5 with Bootstrap Icons
- **Export**: CsvHelper for CSV exports

## 📋 Prerequisites

- .NET SDK 8.0 or higher
- Any modern web browser

## 🔧 Installation & Setup

1. **Clone or extract the project**
```bash
   cd AssetManagementApp
```


# Asset Management System - User Guide

## Quick Start

### First Time Setup
1. Run `dotnet ef database update` to create database
2. Run `dotnet run` to start application
3. Open browser to `https://localhost:5001`

### Navigation
- **Home**: Dashboard with statistics
- **Employees**: Manage employee records
- **Assets**: Manage company assets
- **Assignments**: Assign/return assets
- **Reports**: Export data and view history

## Detailed Workflows

### Adding a New Employee
1. Click "Employees" in sidebar
2. Click "Add New Employee" button
3. Fill in required fields:
   - Full Name *
   - Department *
   - Email * (must be unique)
   - Phone Number (optional)
   - Designation *
   - Active status (checkbox)
4. Click "Save Employee"

### Registering a New Asset
1. Click "Assets" in sidebar
2. Click "Add New Asset" button
3. Fill in details:
   - Asset Name * (e.g., "Dell Laptop XPS 15")
   - Asset Type * (e.g., "Laptop")
   - Make/Model (e.g., "Dell XPS 15 9520")
   - Serial Number * (must be unique)
   - Purchase Date *
   - Warranty Expiry Date (optional but recommended)
   - Condition * (New/Good/Needs Repair/Damaged)
   - Mark as Spare (optional checkbox)
   - Specifications (optional details)
4. Click "Save Asset"
5. Asset automatically set to "Available" status

### Assigning an Asset
1. Click "Assignments" in sidebar
2. Click "New Assignment" button
3. Select Employee from dropdown (only active employees shown)
4. Select Asset from dropdown (only available assets shown)
5. Assignment date defaults to today
6. Add optional notes
7. Click "Assign Asset"
8. Asset automatically changes to "Assigned" status

### Returning an Asset
1. Click "Assignments" in sidebar
2. Find the active assignment
3. Click "Return" button
4. Confirm the return
5. Asset automatically changes back to "Available" status

### Searching and Filtering

#### Employees
- Use search box to filter by:
  - Name
  - Department
  - Email
  - Designation

#### Assets
- **Search box**: Filter by name, serial number, or model
- **Type filter**: Show only specific asset types
- **Status filter**: Show only assets with specific status
- **Clear Filters**: Reset all filters

### Exporting Reports
1. Navigate to "Reports"
2. Choose report type:
   - Assets Report
   - Employees Report
   - Assignments Report
3. Click "Export" button
4. CSV file downloads automatically
5. Open with Excel or any spreadsheet application

### Viewing Assignment History
1. Navigate to "Reports"
2. Scroll to "Assignment History Report"
3. Optionally set date range filters
4. Click "Generate Report"
5. View results in table format

## Dashboard Explained

### Statistics Cards
- **Total Assets**: All assets in system
- **Available**: Assets ready for assignment
- **Assigned**: Assets currently with employees
- **Under Repair**: Assets being serviced
- **Retired**: Assets no longer in use
- **Spare Assets**: Backup equipment

### Assets by Type
- Shows distribution of assets
- Progress bars indicate percentages
- Helps with inventory planning

### Warranty Expiring Soon
- Red badge: ≤ 30 days until expiry
- Yellow badge: 31-90 days until expiry
- Only shows non-retired assets
- Helps prevent warranty lapses

### Quick Actions
- Direct links to common tasks
- Add Employee
- Add Asset
- Assign Asset
- View Assignments

## Tips and Best Practices

### Employee Management
- Mark employees as "Inactive" instead of deleting
- Keeps assignment history intact
- Can reactivate if employee returns

### Asset Management
- Always fill in Serial Number (required for tracking)
- Add Warranty Expiry Date for better planning
- Use Specifications field for detailed info
- Mark spare items for easy identification

### Assignments
- Add notes for context (e.g., "For project X")
- Return assets promptly when not in use
- Check assignment history before reassigning

### Data Integrity
- System prevents:
  - Assigning unavailable assets
  - Assigning to inactive employees
  - Duplicate serial numbers
  - Duplicate email addresses
  - Deleting employees with assignments
  - Deleting assets with active assignments

## Keyboard Shortcuts

- **Ctrl + Click** on links: Open in new tab
- **Tab**: Navigate between form fields
- **Enter**: Submit forms
- **Esc**: Close modals (on some browsers)

## Common Tasks

### Check Which Assets an Employee Has
1. Go to "Assignments"
2. Use browser search (Ctrl+F)
3. Search for employee name
4. See all their current and past assignments

### Find an Asset's Location
1. Go to "Assignments"
2. Search for asset name or serial number
3. Check "Active" assignments to see current holder

### Prepare for Warranty Renewal
1. Check Dashboard "Warranty Expiring Soon" widget
2. Or go to Reports
3. Review assets needing attention
4. Plan purchases or renewals

### Inventory Audit
1. Go to "Reports"
2. Export Assets Report to CSV
3. Open in Excel
4. Sort/filter as needed
5. Verify physical assets match records

## Troubleshooting

### Can't Delete Employee
**Reason**: Employee has assignment records
**Solution**: Mark as "Inactive" instead

### Can't Delete Asset
**Reason**: Asset has assignment history or is currently assigned
**Solution**: Return asset first, or mark as "Retired"

### Duplicate Serial Number Error
**Reason**: Another asset already uses that serial number
**Solution**: Check existing assets, use a different serial number

### Asset Not Showing in Assignment Dropdown
**Reason**: Asset status is not "Available"
**Solution**: Change asset status to "Available" first

### Employee Not in Assignment Dropdown
**Reason**: Employee is marked as "Inactive"
**Solution**: Edit employee and check "Active" checkbox

## Data Backup

### Manual Backup
1. Copy `AssetManagement.db` file
2. Store in safe location
3. To restore: Replace with backup file

### Export to CSV
1. Use Reports feature
2. Export all entities
3. Store CSV files as backup

---

**For technical issues, refer to README.md**