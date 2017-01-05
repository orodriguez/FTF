Feature: Update Note

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: Simple text Update
	Given I created the note number 10 with text 'Buy cheese'
	And I updated the note number 10 with text 'Buy american cheese'
	When I retrieve the note number 10
	Then the note should match:
		| Field        | Value               |
		| Text         | Buy american cheese |
		| CreationDate | Feb 20 1984         |
		| UserName     | orodriguez          |