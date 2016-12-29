﻿Feature: Retrieve Note
	In order to remember a note I saved
	I want to be able to retrieve the information of a note

Scenario: Note Exists
	Given today is 'Feb 20 1984'
	And I signup as 'orodriguez'
	And I signin as 'orodriguez'
	And I created the note number 101 with text 'I was born'
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

Scenario: Note not found
	When I retrieve the note number 101
	Then it should show the error 'Note #101 does not exist'