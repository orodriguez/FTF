Feature: List Joint Tags
	In order to find notes related to specific tags
	I want to be able to list all the tags that have notes in common with a given set of tags

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: 1 Notes, 3 Tags : Read(n1), Piano(n1), Empty()
	Given I created a tag with name 'Empty'
	And I created a note with text '#Read a book about #Piano performance'
	When I list all tags that joint the tag 'Read'
	Then the tags list should match:
		| Name  | NotesCount |
		| Read  | 1          |
		| Piano | 1          |

Scenario: 3 Notes, 3 Tags : Read(n1, n3), Programming(n1, n2), FTF(n2, n3)
	Given I created a note with text '#Read article about #Programming'
	And I created a note with text 'Write sample application #Programming #FTF'
	And I created a note with text '#FTF #Read about design principles'
	When I list all tags that joint the tag 'Read'
	Then the tags list should match:
		| Name        | NotesCount |
		| Read        | 2          |
		| Programming | 1          |
		| FTF         | 1          |

Scenario: 2 Notes, 3 Tags : Buy(n1), Car(n1), Mary(n2)
	Given I created a note with text '#Buy tire #Car'
	And I created a note with text '#Buy gift for #Mary'
	When I list all tags that joint the tag 'Mary'
	Then the tags list should match:
		| Name | NotesCount |
		| Mary | 1          |
		| Buy  | 1          |