Feature: 04EditingJourneyPlanner


Scenario: Verify editing a journey
	Given I have navigated to the TFL journey planner
	When I enter 'Stratford' in the From field
	And I enter 'North Greenwich' in the To field
	And I click the Plan My Journey button
	And I edit the journey details
	Then I should be able to amend the journey
