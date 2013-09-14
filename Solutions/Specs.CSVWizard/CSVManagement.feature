Feature: CSVManagement
	In order to manage CSVs
	As a user
	I want to be able to load csv files into memory

@requiresClean
Scenario: Load CSV file
	Given I have a well formed CSV file
	When I load the CSV file
	Then I should have the correct structure in memory
