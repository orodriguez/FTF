Feature: Retrieve Note
	In order to remember a note I saved
	I want to be able to retrieve the information of a note

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: Note Exists
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

Scenario: Note from another user
	Given I created the note number 101 with text 'Note from another user'
	And I signup and signin as 'anotheruser'
	When I retrieve the note number 101
	Then it should show the error 'Note #101 does not exist'

Scenario: Note not found
	When I retrieve the note number 101
	Then it should show the error 'Note #101 does not exist'

Scenario: Note updated
	Given I created the note number 10 with text 'Buy cheese'
	And I updated the note number 10 with text 'Buy american cheese'
	When I retrieve the note number 10
	Then the note should match:
		| Field        | Value               |
		| Text         | Buy american cheese |
		| CreationDate | Feb 20 1984         |
		| UserName     | orodriguez          |