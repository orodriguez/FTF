Feature: List Tags
	In order to how I have my notes clasified
	I want to be able to see all the tags I have registered

Background:
	Given today is 'Feb 20 1984' 
	Given I signup as 'orodriguez'
	And I signin as 'orodriguez'

Scenario: One Note with New Tag
	Given I created a note with text '#Read Athlas Shrugged'
	When I list all tags
	Then the tags list should match:
		| Name | NotesCount |
		| Read | 1          |

Scenario: Two Notes with the same Tag
	Given I created a note with text '#Buy cheese'
	And I created a note with text '#Buy nachos'
	When I list all tags
	Then the tags list should match:
         | Name | NotesCount |
         | Buy  | 2          |

Scenario: One Tag from different user
	Given I created a note with text '#Buy cheese'
	And I signup as 'anotheruser'
	And I signin as 'anotheruser'
	When I list all tags
	Then the tags list should be empty