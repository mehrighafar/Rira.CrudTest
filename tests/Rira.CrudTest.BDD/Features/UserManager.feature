Feature: User Manager
As a an operator I wish to be able to Create, Update, Delete users and list all users
	
	Background:
		Given I am a client

	Scenario: 1.user get successfully
	Getting the list of all stored users
		Given The repository is seeded with random users
		When I make a GET request to user
        Then The response should contain the seeded users

	Scenario: 2.user get created successfully
	Creating a user successfully in database
		Given I have generated a random user
        When I make a POST request with the user to user
        Then The response should match the sent user
	
	Scenario: 3.user get updated successfully
	Update a stored user successfully
		Given The repository is seeded with random users
		When I make a PUT request with updated data to user
		Then The response should match the updated user

	Scenario: 4.user get removed successfully
	Delete a user successfully from database 
		Given The repository is seeded with random users
		When I make a DELETE request for that user to user
		Then The response should be empty
