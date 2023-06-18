# TfL Tech Lead Coding Challenge

## Dataverse Solutions

[Managed Solution](DataverseSolution/TfLCodingChallenge_1_0_1_1_managed.zip)  
[Unmanaged Solution](DataverseSolution/TfLCodingChallenge_1_0_1_1.zip)

## Business Unit Configuration Data

Business units are imported with the Power Platform CLI.  
  
Unless there is one already, create an auth profile for the correct org.  
```
pac auth create --url https://bwcodingchallenge.crm4.dynamics.com
```
Select the correct auth profile. Replace 1 with the correct profile.  
```
pac auth select -i 1
```
Import the business units  
```
pac data import -d .\data.zip
```

Place customer service agents in their corresponding business.  
 - Underground
 - Buses
 - Overground

 Place managers in the "Customer Service Manager" BU.  
 Place confidential case team members in the "Confidential Case Team" BU.  
 Place escalationt team members on the "Escalation Team" BU.  

## Security Role Manual Assignment

 Grant the "Buses" team the "Customer Service Agent" security role.  
 Grant the "Overground" team the "Customer Service Agent" security role.  
 Grant the "Underground" team the "Customer Service Agent" security role.  

 Grant the "Customer Service Manager" team the "Customer Service Manager" security role.  
 Grant the "Escalation Team" team the "Escalation Team" security role.  
 Grant the "Confidential Case Team" team the "Confidential Case Team" security role.  
 

## Case Documents

Image attachments associated with the case, contact, and email are included in the follow up.

## Plugin: Stop Case Resolution

StopCaseResolutionPlugin allows case resolution only for users with the "Customer Service Manager" role.  

Tests use NUnit and FakeItEasy. 

## Command Bar JavaScript

JavaScript is used by a command bar button to create follow ups.

## Power Automate

Power Automate is used to copy documents to a follow up.

## Model Driven App

There is a single model driven app called CCO.
The sitemap only shows tables that the user has access to.
