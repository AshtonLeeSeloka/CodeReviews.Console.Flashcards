﻿using FlashcardStack.AshtonLeeSeloka.Services;
using FlashcardStack.AshtonLeeSeloka.Views;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;
namespace FlashcardStack.AshtonLeeSeloka.Controllers;

internal class HomeController
{
	private readonly HomeView _homeView = new HomeView();
	private readonly StudyController _studyController = new StudyController();

	public void Start() 
	{
		while (true) 
		{
			var selection =_homeView.MainMenu();

			switch (selection) 
			{
				case MainMenu.View_Reports:
					break;
				case MainMenu.Manage_Stacks:
					break;
				case MainMenu.Study:
					_studyController.StartStudying();
					break;
				case MainMenu.Exit:
					Environment.Exit(0);
					break;
			}
		}
	}
}
