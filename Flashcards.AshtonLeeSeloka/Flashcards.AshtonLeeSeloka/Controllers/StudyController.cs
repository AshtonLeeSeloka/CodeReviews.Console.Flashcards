﻿using Flashcards.AshtonLeeSeloka.DTO;
using Flashcards.AshtonLeeSeloka.Views;
using FlashcardStack.AshtonLeeSeloka.Models;
using FlashcardStack.AshtonLeeSeloka.Services;
using static FlashcardStack.AshtonLeeSeloka.MenuEnums.MenuEnums;
namespace FlashcardStack.AshtonLeeSeloka.Controllers;

internal class StudyController
{
	private readonly DataService _dataService = new DataService();
	private readonly UIViews _views = new UIViews();
	public void StartStudying()
	{
		Console.Clear();
		List<StackModel> AvailableStacks = _dataService.GetAvailableStacks();
		StackModel selection = _views.SelectStackView(AvailableStacks, "Select [green]Stack[/] to [cyan]study[/]");
		List<Card> cards = _dataService.GetCards(selection);
		var menuSelection = _views.StudyOptions();

		switch (menuSelection)
		{
			case StudyOptions.View_All_Cards:
				ViewAllCards(cards);
				break;
			case StudyOptions.Play:
				Play(cards);
				break;
			case StudyOptions.Exit:
				break;
		}
	}

	public void Play(List<Card> cards)
	{
		if (cards.Count == 0)
		{
			Console.WriteLine("Stack contains no cards \nAdd cards before playing\nPpress any key to select a different stack");
			Console.ReadKey();
			StartStudying();
		}

		int counter = 0;
		string? stack = cards[0].StackName;
		int ID = _dataService.GetStackID(stack);
		Console.Clear();
		int score = 0;
		List<string>? Questions = new List<string>();

		foreach (Card card in cards)
		{
			Questions.Add(card.Back);
		}

		while (cards.Count != 0 && counter < 5)
		{
			Console.Clear();
			Random rnd = new Random();
			int index = rnd.Next(cards.Count);
			_views.DisplayCardFront(cards[index].Front);
			var answer = _views.QuestionAnswer(Questions);
			if (answer.Equals(cards[index].Back))
			{
				score++;
				Console.WriteLine("Correct!!!, Press any key to continue");
				Console.ReadKey();
			}
			else
			{
				Console.WriteLine("Incorrect, Press any key to continue");
				Console.ReadKey();
			}

			cards.RemoveAt(index);
		}
		DateTime dateTime = DateTime.Now.ToUniversalTime();
		_dataService.InsertScore(stack, dateTime, score, ID);
		Console.WriteLine($"Finale score: {score}");
		Console.ReadKey();
	}

	public void ViewAllCards(List<Card> cards)
	{
		_views.ViewCardsAsTable(cards);
		Console.WriteLine("Press Any Key to return.");
		Console.ReadKey();
		StartStudying();
	}
}
