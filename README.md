# WHAT backend

## 1. About the project

The “WHAT” project (who is absent today) is intended to manage the educational process in educational centers where the student-mentor system operates. The service allows you to track progress, attendance, materials studied, as well as view lists of students, mentors and other useful information about the educational process.

## 2. Where to find UI part of the project

Here is the UI part of project "WHAT": https://github.com/ita-social-projects/what-front

---

## Table of Contents

- [Documentation](#Documentation)
  - [Required to install](#Required-to-install)
  - [Setup](#Setup)
  - [How to run local](#How-to-run-local)
  - [Git flow](#git-flow)
- [FAQ](#faq)
- [License](#license)

---

## 3. Documentation

### Required to install

* netcoreapp 3.1
* MySql 8.0.21

### Setup

- clone this repo to your local machine using git clone
- create an .env file to manage your application secrets:
  https://github.com/tonerdo/dotnet-env
- create an email account for the notification service and write it to .env file
- use our scripts to create and fill the database with test data
- add connection string for created database to .env file
 
### How to run local

- open CharlieBackend.sln
- run your local or remote data base
- run with IDE or use cmd "**dotnet run**" command in every folder
  where we have **.csproj** file (for now it is AdminPanel and API)
- open _http://localhost:5000_ to view API Swagger in a browser
- open _https://localhost:5003_ or _http://localhost:5002_ to view UI AdminPanel

### Git flow

- Project have **dev**, **release** and **yourBranchForChanges** branches.  
- All **yourBranchForChanges** branches must be merged into **dev** branch.
- Once a week, the **dev** branch is merged into the **release** branch.

---

## FAQ

---

## License

- Copyright 2020 **©SoftServe IT Academy**
