Feature: Delete

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: Simple
	Given I created the note number 101 with text 'I was born'
	And I deleted the note 101
	When I retrieve the note number 101
	Then it should show the error 'Note with id #101 does not exist'

Scenario: With Tags
	Given I created the note number 10 with text '#Buy cheese'
	And I deleted the note 10
	When I list all tags
	Then the tags list should match:
         | Name | NotesCount |
         | Buy  | 0          |