Feature: Create Note

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: Simple
	Given I created the note number 101 with text 'I was born'
	When I retrieve the note number 101
	Then the note should match:
		| Field			| Value       |
		| Text			| I was born  |
		| CreationDate	| Feb 20 1984 |
		| UserName		| orodriguez  |

Scenario: With Tags
	Given I created the note number 101 with text '#Buy cheese at #SuperMarket'
	When I retrieve the note number 101
	Then the note should contain the tags:
		| Name        |
		| Buy         |
		| SuperMarket |

Scenario: Empty Text
	When I create a note with text ''
	Then it should show the error 'Note can not be empty'
