[![DOI](https://zenodo.org/badge/510174176.svg)](https://doi.org/10.5281/zenodo.18731966)

# ABSENSI – Attendance Management System

ABSENSI is a web-based attendance management system built using ASP.NET MVC (C#) and Entity Framework.  
This project provides schedule management and attendance tracking with role-based access control.

---

## Overview

ABSENSI is designed to digitize employee attendance processes.  
The system separates roles into Administrator and User (Employee).

- Administrators manage users and schedules.
- Users record attendance within an assigned time window.

---

## Features

### Administrator
- Add and delete employees
- Create and manage attendance schedules
- Update and delete schedule events
- View calendar-based attendance
- Export daily attendance reports to Excel (.xlsx)

### User
- Secure login with encrypted password
- View assigned schedule
- Submit attendance within scheduled time
- View personal attendance history

---

## Technology Stack

- ASP.NET MVC
- C#
- Entity Framework
- SQL Server
- BCrypt.Net (password hashing)
- EPPlus (Excel report generation)
- Google Maps Geocoding API (reverse geolocation)

---

## Security

- Passwords are encrypted using BCrypt
- Session-based authentication
- Role-based access (Admin and User)
- Attendance submission restricted to scheduled time range

---

## Attendance Logic

- Attendance can only be submitted within scheduled time
- Past schedules cannot be modified
- Each schedule is linked to a specific user
- Attendance status is recorded as Present or Absent

---

## Project Structure

ABSENSI/
- Controllers
  - AdminController.cs
  - HomeController.cs
  - LoginController.cs
- Models
- ViewModel
- Context
- ABSENSI.sln

---

## Installation

1. Clone the repository:
   git clone https://github.com/piopratama/ABSENSI.git

2. Open the solution file in Visual Studio:
   ABSENSI.sln

3. Configure the database connection string in Web.config

4. Restore NuGet packages if required

5. Run the project using IIS Express

---

## Requirements

- Visual Studio
- .NET Framework
- SQL Server
- NuGet Packages:
  - EntityFramework
  - BCrypt.Net
  - EPPlus

---

## Citation

If you use this software in academic work, please cite:

Pratama, I. W. P. (2025). ABSENSI: Attendance Management System (Version 1.0). Zenodo. https://doi.org/10.5281/zenodo.18731966

---

## License

This project is released under the MIT License.

---

## Author

I Wayan Pio Pratama  
GitHub: https://github.com/piopratama
