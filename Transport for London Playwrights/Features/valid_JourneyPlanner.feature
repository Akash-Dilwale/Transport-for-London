Feature: ValidJourneyPlanner


Scenario: Verify a valid journey
	Given I have navigated to the TFL journey planner
	When I enter 'Stratford' in the From field
	And I enter 'North Greenwich' in the To field
	And I click the Plan My Journey button
	Then I should see the journey results