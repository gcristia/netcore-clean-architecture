﻿using BuberDinner.Domain.Common.Menu.ValueObjects;
using BuberDinner.Domain.Common.Models;

namespace BuberDinner.Domain.Common.Menu.Entities;

public sealed class MenuSection : Entity<MenuSectionId>
{
    private readonly List<MenuItem> _items = new();
    public string Name { get; }
    public string Description { get; }

    public IReadOnlyCollection<MenuItem> Items => _items.AsReadOnly();

    public MenuSection(MenuSectionId menuSectionId, string name, string description) : base(menuSectionId)
    {
        Name = name;
        Description = description;
    }

    public static MenuSection Create(string name, string description)
    {
        return new MenuSection(MenuSectionId.CreateUnique(), name, description);
    }
}
