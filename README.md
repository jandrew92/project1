# SIMULINGUA

## Project Description

REST API designed for a language-learning services website. 

## Technologies Used

* .NET Framework
* ADO.NET
* Coded in C#
* Written using Visual Studio
* Microsoft SQL Server Database
* Tested via the Swagger interface

## Features

* Communicates with a SQL database through HTTP requests. 
* Allows new users to join the site with email, name, and birthday.
* Allows existing users to change their name or email address.
* Allows users to specify information about their linguistic abilities and purchase language courses. 
* Allows admins to add or delete courses, update prices, check user list, and add language options.
* Allows users to review their reciepts of purchased courses.

To-do list:
* Student-to-student matching service, for tutoring and group study.
* Ability to search for a specific course by language and skill level.

## Getting Started

1. Clone the repository.
  git clone https://github.com/jandrew92/project1.git
2. import the SimulinguaDB.bak file to your SQL Server
  Right click 'Databases' in the Object Explorer
  Click 'Restore Database...'
  Choose 'Device', '...' and 'add'
  Navigate to the SimulinguaDB.bak file, select it, and click 'OK'
  Click 'OK' then 'OK' again
3. Modify Simulingua/ConnectionString.xml so that it points to your SQL server.
4. Open the Simulingua folder in Visual Studio and launch Swagger through ISS Express.

## Usage

Use the following HTTP methods to alter the database:

* /AdminActions/
	add-new-language
	add-new-language-course
	update-language-course-prince
	delete-language-course
	fetch-all-user-data
	fetch-all-user-receipts
	
* /StoreActions/
    user-action-purchase-language-course
	fetch-store-menu
	
* /UserInfo/
	new-user
	add-new-target-language
	update-user
	update-target-language-proficiency
	fetch-user-info
	fetch-user-target-languages
	fetch-user-reciepts
	delete-user-target-language