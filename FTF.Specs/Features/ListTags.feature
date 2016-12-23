Feature: List Tags
	In order to how I have my notes clasified
	I want to be able to see all the tags I have registered

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

Scenario: Note with duplicated Tag
	Given I created a note with text '#Read a book about how to #Read'
	When I list all tags
	Then the tags list should match:
         | Name | NotesCount |
         | Read | 1          |