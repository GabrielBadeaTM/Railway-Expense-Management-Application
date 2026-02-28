# 🚝 DepoHelper - Railway Expense Management Application

A comprehensive desktop application designed to manage the activities, logistics, and financial expenses of a railway depot.
Built to track purchases of parts, external services, and invoices, accurately linking them to specific locomotives, wagons, or administrative cost centers.
Developed to provide both operational depot management and structured financial analysis.

## 🛠️ Technologies Used

- **C#** & **.NET Framework** (Windows Forms)
- **Microsoft SQL Server** (Relational Database)
- **ADO.NET** (SqlConnection, SqlCommand, SqlDataReader for DB communication)
- **Visual Studio 2022**

## ⚙️ How It Works

1. **Cost Center Management:** The system strictly categorizes expenses into four main areas: Locomotive Maintenance (parts & services), Wagon Maintenance (parts only, tied to client firms), Depot Maneuvers (consumables/services), and Central (administrative costs like utilities).
2. **Database-Driven Calculations:** To ensure maximum performance, all financial aggregations (monthly/yearly totals) are executed directly at the SQL Server level using advanced queries (e.g., `SUM`, `OVER()`, `UNION ALL`), rather than processing data in C#. (This was a project requirement)
3. **Invoice Processing:** Users can input invoices, select the supplier, and dynamically allocate purchased parts or services directly to specific vehicles or cost centers. The save mechanism is transactional, preventing incomplete invoice entries.
4. **Dynamic Reporting & Filtering:** The central dashboard features multiple modules (Tabs) where users can generate custom financial reports by filtering data based on specific dates (DateTimePickers) and target vehicles.
5. **Strict Data Integrity:** The application enforces relational database rules. For instance, you cannot delete a part or a locomotive from the settings if it is already tied to an existing invoice, preserving the integrity of historical financial data.

## 📝 Notes

Developed in October 2025 as a university project, tailored to meet the practical requirements of my father's company.

🧯 I should mention that the code is written... quite poorly. But it works. The project was completed for university under very tight time constraints, and it didn’t make sense to go back and refactor the code. In any case, the application serves both its educational and practical purposes, as it is fully functional.

P.S. I would add a release, but for now I can’t figure out why it causes errors on other PCs...
