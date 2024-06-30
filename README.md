# CSVReaderTask

CSVReaderTask is a desktop application built with WPF that allows users to interact with CSV data efficiently. It provides functionalities to read CSV files, import data into a SQL Server database, and export data to various formats like Excel (.xlsx) and XML.

## Features

- **Read CSV File:** Seamlessly upload CSV files and store the data into a SQL Server database using Entity Framework for robust data management.
- **Export to Excel:** Export data from the database to well-formatted Excel files (.xlsx), enabling easy sharing and analysis.
- **Export to XML:**  Export data from the database to XML files (.xml), providing flexibility in data exchange.
- **Filtering Data:**  View and filter data effortlessly using a WPF DataGrid with advanced filtering options for precise data exploration.
- **Error Handling:**  Handles potential errors such as bad data and missing fields in CSV files gracefully with informative notifications to ensure data integrity.

## Requirements

- **.NET Framework:**  Specify the minimum required version of the .NET Framework for compatibility.
- **SQL Server:** Requires a running instance of SQL Server for database storage.
- **Syncfusion (Optional):**  If applicable, specify the Syncfusion library used for Excel file handling.

## Installation

1. **Clone the repository:**

`git clone https://github.com/StopSandal/EventSeller`

2. **Set up the database:**

Ensure your SQL Server instance is running.

Modify the connection string in RegistrationServiceExtension.cs to match your SQL Server instance credentials.

Run migrations.

3. **Build and run:**
   
Build and run the application from Visual Studio or using command-line tools.

## Usage

Read CSV File: Click the "Read CSV File" button in the main window, select your CSV file, and initiate the import process.

Export to Excel: Filter your data in the DataGrid as needed, then click the "Export to Excel" button to generate an Excel file.

Export to XML: Similar to Excel export, filter your data and use the "Export to XML" button to create an XML file.

## Configuration

Database Settings: Adjust the database connection settings within the RegistrationServiceExtension.cs file if required.
