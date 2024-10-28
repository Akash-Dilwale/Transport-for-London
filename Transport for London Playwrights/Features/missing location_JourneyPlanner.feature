Feature: MissingJourneyPlanner


Scenario: Verify missing location handling
	Given I have navigated to the TFL journey planner
	When I click the Plan My Journey button
	Then I should see a message indicating missing locations