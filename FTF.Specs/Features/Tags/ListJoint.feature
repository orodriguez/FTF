Feature: List Joint Tags
	In order to find notes related to specific tags
	I want to be able to list all the tags that have notes in common with a given set of tags

Background:
	Given today is 'Feb 20 1984' 
	And I signup and signin as 'orodriguez'

Scenario: 1 Notes, 3 Tags : t1(n1), t2(n1), t3()
	Given I created a tag with name 'EmptyTag'
	And I created a note with text '#Read a book about #Piano performance'
	When I list all tags that joint the tag 'Read'
	Then the tags list should match:
		| Name  | NotesCount |
		| Read  | 1          |
		| Piano | 1          |

Scenario: 3 Notes, 3 Tags : t1(n1, n2), t2(n2, n3), t1(n3, n1)
	Given I created a note with text '#Read artible about #Programming'
	And I created a note with text 'Write sample application #Programming #FTF'
	And I created a note with text '#FTF #Read about design principles'
	When I list all tags that joint the tag 'Read'
	Then the tags list should match:
		| Name        | NotesCount |
		| Read        | 2          |
		| Programming | 2          |
		| FTF         | 2          |