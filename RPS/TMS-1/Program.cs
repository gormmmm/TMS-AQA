using TMS_1;

// Main - Program 

using System;
using TMS_1;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Rock Paper Scissors!");
        
        int rounds = GetRoundsFromPlayer();
        
        var myGame = new Game(rounds);
        myGame.Play();
    }
    
    static int GetRoundsFromPlayer()
    {
        int roundsToPlay;
        while (true)
        {
            Console.WriteLine("How many rounds do you want to play?");
            string input = Console.ReadLine();
            
            if (int.TryParse(input, out roundsToPlay) && roundsToPlay > 0)
            {
                return roundsToPlay; 
            }
            
            Console.WriteLine("Invalid input. Please enter a valid number greater than 0.");
        }
    }
}