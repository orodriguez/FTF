Feature: Create Note

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: Note without Tags
	Given I created the note number 101 with text 'I was born'
	When I retrieve the note number 101
	Then the note should match:
		| Field			| Value       |
		| Text			| I was born  |
		| CreationDate	| Feb 20 1984 |
		| UserName		| orodriguez  |

Scenario: Note with Tags
	Given I created the note number 101 with text '#Buy cheese at #SuperMarket'
	When I retrieve the note number 101
	Then the note should contain the tags:
		| Name        |
		| Buy         |
		| SuperMarket |
