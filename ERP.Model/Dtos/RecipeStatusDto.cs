using ERP.Domain.Command;

namespace ERP.Domain.Dtos;

public class RecipeStatusDto : AuditDto
{
    public string Name { get; set; }
}