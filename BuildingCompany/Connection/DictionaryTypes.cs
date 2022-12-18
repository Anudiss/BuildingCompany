using System.Collections.Generic;

namespace BuildingCompany.Connection
{
    public static class Positions
    {
        public static Position Worker => AllPositions[0];
        public static Position Storekeeper => AllPositions[1];
        public static Position Manager => AllPositions[2];
        public static Position HR => AllPositions[3];

        public static readonly List<Position> AllPositions = new List<Position>
        {
            new Position("Рабочий"),
            new Position("Кладовщик"),
            new Position("Менеджер"),
            new Position("Кадровик")
        };
    }
    public static class Roles
    {
        public static Role Client => AllRoles[0];
        public static Role Admin => AllRoles[1];
        public static Role Employee => AllRoles[2];

        public static readonly List<Role> AllRoles = new List<Role>()
        {
            new Role("Клиент"),
            new Role("Админ"),
            new Role("Сотрудник")
        };
    }
    public static class Genders
    {
        public static Gender Man => AllGenders[0];
        public static Gender Woman => AllGenders[1];

        public static readonly List<Gender> AllGenders = new List<Gender>()
        {
            new Gender("Мужской"),
            new Gender("Женский")
        };
    }
    public static class Stages
    {
        public static Stage New => AllStages[0];
        public static Stage Processing => AllStages[1];
        public static Stage ToPay => AllStages[2];
        public static Stage Paid => AllStages[3];
        public static Stage Building => AllStages[4];
        public static Stage Done => AllStages[5];

        public static readonly List<Stage> AllStages = new List<Stage>()
        {
            new Stage("Новый"),
            new Stage("Обработка"),
            new Stage("К оплате"),
            new Stage("Оплачен"),
            new Stage("Стройка"),
            new Stage("Готово")
        };
    }

    public class Position
    {
        public string Name { get; }

        public Position(string name) =>
            Name = name;
    }
    public class Role
    {
        public string Name { get; }

        public Role(string name) =>
            Name = name;
    }
    public class Gender
    {
        public string Name { get; }

        public Gender(string name) =>
            Name = name;

        public override string ToString() => Name;
    }
    public class Stage
    {
        public string Name { get; }

        public Stage(string name) =>
            Name = name;
    }
}
