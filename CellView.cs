
public class CellView
{
    // Whether this cell is visible to the player //Encapsulation of the cell's visibility state, allowing controlled access to whether the cell is visible to the player
    public bool IsVisible { get; private set; }

    // Value of a visible cell
    public int Value { get; private set; }

    // Constructor
    public CellView()
    {
        IsVisible = false;
        Value = 0;
    }

    // Update the visible state of this cell
    public void SetVisible(int value)
    {
        IsVisible = true;
        Value = value;
    }

    // Hide the cell
    public void SetHidden()
    {
        IsVisible = false;
        Value = 0;
    }

    // Return the symbol displayed to the player
    public char GetSymbol()
    {
        // Hidden cells are displayed as '?'
        if (!IsVisible)
        {
            return '?';
        }

        // Fog only uses ordinary stones
        return Value switch
        {
            0 => '.',  // Empty cell
            1 => 'X',  // Player 1 Ordinary Stone
            3 => 'O',  // Player 2 Ordinary Stone
            _ => '?'   // Unknown or unsupported value
        };
    }
}
