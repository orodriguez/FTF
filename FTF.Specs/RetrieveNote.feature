Feature: Retrieve Note
	In order to remember a note I saved
	I want to be able to retrieve the information of a note

Scenario: Simple Note
	Given today is 'Feb 20 1984'
	And I created a note #101 with text 'I was born'
	When I retrieve the note #101
	Then the note should match:
		| Field        | Value       |
		| Text         | I was born  |
		| CreationDate | Feb 20 1984 |