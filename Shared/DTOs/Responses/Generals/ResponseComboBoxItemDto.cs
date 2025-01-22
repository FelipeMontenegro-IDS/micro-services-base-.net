namespace Shared.DTOs.Responses.Generals;

public class ResponseComboBoxItemDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }

    public ResponseComboBoxItemDto(Guid id, string text)
    {
        Id = id;
        Text = text;
    }
}