Feature: List Tags
	In order to how I have my notes clasified
	I want to be able to see all the tags I have registered

Scenario: Create a Note
	Given I created a note with text '#Read Athlas Shrugged'
	When I list all tags
	Then the tags list should match:
		| Name | NotesCount |
		| Read | 1          |