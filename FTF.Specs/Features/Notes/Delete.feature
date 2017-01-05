Feature: Delete

Scenario: Simple
	Given I created the note number 101 with text 'I was born'
	And I deleted the note 101
	When I retrieve the note number 101
	Then it should show the error 'Note #101 does not exist'