Feature: List Joint Tags
	In order to find notes related to specific tags
	I want to be able to list all the tags that have notes in common with a given set of tags

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: One Tag without notes and 2 Tags related to the same note
	Given I created a tag with name 'EmptyTag'
	And I created a note with text '#Read a book about #Piano performance'
	When I list all tags that joint the tag 'Read'
	Then the tags list should match:
		| Name  | NotesCount |
		| Read  | 1          |
		| Piano | 1          |