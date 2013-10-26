Feature: Entering a Journal entry
	To write my Diary on a Windows 7 machine
	I want to enter a simple text entry in a form
	so it  will be stored in my day one repository on DropBox


Scenario: Storing a journal entry in dayone
	Given I enter a new entry on the 17-07-2013 at 15:20 with the following text
	"""
	Das ist mein erster Eintrag 
	"""
	When I click on Save
	Then a new doentry file is visible in my dropbox folder

