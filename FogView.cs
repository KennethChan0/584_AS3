
using System;

public class FogView : IObserver // Represents a player's filtered view of the board, showing only visible cells
{
    // The player who owns this filtered view
    private readonly Player viewer;

    // Store the player's filtered board
    private readonly PlayerBoardView view;

    // Constructor
    public FogView(Player viewer, Board board)
    {
        if (viewer == null)
        {
            throw new ArgumentNullException(nameof(viewer));
        }

        if (board == null)
        {
            throw new ArgumentNullException(nameof(board));
        }

        this.viewer = viewer;

        // Create a separate board view for this player //individual board view for the player, allowing them to see only their own visible cells
        view = new PlayerBoardView(board.Rows, board.Columns);

        // Calculate initial visibility
        Update(board);
    }

    // Implement IObserver interface //Polymorphism //not changing the ture board, just updating the filtered view for the player
    public void Update(Board board)
    {
        // Refresh the filtered board for this player
        view.Update(board, viewer);
    }

    // Display the player's filtered board
    public void Display()
    {
        Console.WriteLine($"--- {viewer.Name}'s Fog View ---");
        view.Display();
    }

    // Allow the game to identify the view owner
    public Player Viewer => viewer;

    // Read-only access for testing
    public PlayerBoardView View => view;
}
