Feature: ReceantTabJourneyPlanner

Scenario: Verify recents tab functionality
	Given I have navigated to the TFL journey planner
	When I enter 'Stratford' in the From field
	And I enter 'North Greenwich' in the To field
	And I click the Plan My Journey button
	And navigated to the TFL journey planner
	When I click the recents tab
	Then I should see the list of recent journeys