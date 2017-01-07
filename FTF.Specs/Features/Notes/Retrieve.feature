Feature: Retrieve Note
	In order to remember a note I saved
	I want to be able to retrieve the information of a note

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: Note from another user
	Given I created the note number 101 with text 'A Note'
	And I signup and signin as 'anotheruser'
	When I retrieve the note number 101
	Then it should show the error 'Note with id #101 does not exist'

@exception
Scenario: Note not found
	When I retrieve the note number 101
	Then it should show the error 'Note with id #101 does not exist'