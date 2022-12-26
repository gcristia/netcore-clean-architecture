using BuberDinner.Domain.Common.Dinner.ValueObjects;
using BuberDinner.Domain.Common.Host.ValueObjects;
using BuberDinner.Domain.Common.Menu.Entities;
using BuberDinner.Domain.Common.Menu.ValueObjects;
using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.Common.Menu;

public sealed class Menu : AggregateRoot<MenuId>
{
    private readonly List<MenuSection> _sections = new();
    private readonly List<DinnerId> _dinnerIds = new();
    private readonly List<MenuReviewId> _menuReviewIds = new();
    public string Name { get; }
    public string Description { get; }
    public float AverageRating { get; }

    public IReadOnlyCollection<MenuSection> Sections => _sections.AsReadOnly();

    public HostId HostId { get; }

    public IReadOnlyCollection<DinnerId> DinnerIds => _dinnerIds.AsReadOnly();

    public IReadOnlyCollection<MenuReviewId> MenuReviewIds => _menuReviewIds.AsReadOnly();

    public DateTime CreatedDateTimet { get; }
    public DateTime UpdatedDateTime { get; }

    public Menu(MenuId menuId, string name, string description, HostId hostId, DateTime createdDateTimet,
        DateTime updatedDateTime) : base(menuId)
    {
        Name = name;
        Description = description;
        HostId = hostId;
        CreatedDateTimet = createdDateTimet;
        UpdatedDateTime = updatedDateTime;
    }

    public static Menu Create(string name, string description, HostId hostId)
    {
        return new Menu(MenuId.CreateUnique(), name, description, hostId, DateTime.UtcNow, DateTime.UtcNow);
    }
}
