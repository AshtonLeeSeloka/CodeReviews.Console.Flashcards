﻿using Flashcards.AshtonLeeSeloka.DTO;
using Flashcards.AshtonLeeSeloka.Services;
using Flashcards.AshtonLeeSeloka.Views;
using FlashcardStack.AshtonLeeSeloka.MenuEnums;
using FlashcardStack.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Services;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;

namespace Flashcards.AshtonLeeSeloka.Controllers;

internal class StackController
{
	private readonly DataService _dataService = new DataService();
	private readonly UIViews _views = new UIViews();
	private readonly ValidationService _validationService = new ValidationService();
	
	public void MainMenu()
	{
		var selection = _views.ManageStacksMenu();
		switch (selection) 
		{
			case ManageStacks.Create_New_Stack:
				break;
			case ManageStacks.Manage_Existing_Stacks:
				ManageExistingStack();
				break;
			case ManageStacks.Exit:
				break;
		}
	}

	public void ManageExistingStack() 
	{
		List<StackModel> AvailableStacks = _dataService.GetAvailableStacks();
		StackModel selection = _views.SelectStackView(AvailableStacks, "Select the [green]Stack[/] to Manage");
		List<CardDTO> cards = _dataService.GetCards(selection);
		_views.ViewCardsAsTable(cards);
		var menuSelection = _views.ManageExistingStacksMenu(selection);

		switch (menuSelection)
		{
			case MenuEnums.ManageExistingStack.Create_New_Card:
				CreateNewCard(selection);
				break;
			case MenuEnums.ManageExistingStack.Edit_Cards:
				EditCard(selection);
				break;
			case MenuEnums.ManageExistingStack.Delete_Cards:
				break;

			case MenuEnums.ManageExistingStack.Exit:
				break;
		}
	}

	public void CreateNewCard(StackModel selection) 
	{
		string front = _views.PromptUser("Enter the [red]Front[/] of the Card, or Type '0' to Exit");
		if (front.Equals("0"))
			return;

		string back = _views.PromptUser("Enter the [red]Back[/] of the Card, or Type '0' to Exit");
		if (back.Equals("0"))
			return;

		int? foreignKey = selection.Stack_ID;
		_dataService.InsertCard(front, back, foreignKey);
		List<CardDTO> cards = _dataService.GetCards(selection);
		_views.ViewCardsAsTable(cards);
		Console.WriteLine("Card Added Succesfully, press any key to return");
		Console.ReadLine();
	}

	public void EditCard(StackModel selection)
	{
		List<CardDTO> cards = _dataService.GetCards(selection);
		_views.ViewCardsAsTable(cards);
		int cardIndex = _validationService.getIndex(cards);
		if (cardIndex == 0)
			return;

		string? front = _views.PromptUser("Enter the front of the Card or Type x to leave the same");
		if (front.Equals("x"))
			front = cards[cardIndex - 1].Front;

		string? back = _views.PromptUser("Enter the bacck of the Card or Type x to leave the same");
		if (back.Equals("x"))
			back = cards[cardIndex - 1].Back;

		_dataService.EditCard(front, back, cards[cardIndex-1].ID);
		cards = _dataService.GetCards(selection);
		_views.ViewCardsAsTable(cards);
		Console.WriteLine("Card Added Succesfully, press any key to return");
		Console.ReadLine();
	}


}
