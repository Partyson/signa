﻿namespace signa.FunctionalTests.Helpers;

public static class MockUsersFio
{
    private static readonly Random Random = new ();
    private static readonly List<string> Data =
    [
        "Самадов Фируз Фаррухович",
        "Лазарев Алексей Андреевич",
        "Смоляков Дмитрий Станиславович",
        "Горбатенко Данил Андреевич",
        "Киселев Егор Алексеевич",
        "Шайхутдинов Рамазан Шамильевич",
        "Глазырин Григорий Сергеевич",
        "Микулич Дмитрий Олегович",
        "Замараев Дмитрий Алексеевич",
        "Цветков Тимофей Алексеевич",
        "Баталов Лев Евгеньевич",
        "Ермаков Егор Витальевич",
        "Бельтиков Дмитрий Викторович",
        "Талантбекова Тумар Талантбековна",
        "Таипов Тимур Фаизович",
        "Гильманов Эмиль Наилевич",
        "Никифоров Дмитрий Андреевич",
        "Размадзе Лев Владимирович",
        "Булатов Иван Олегович",
        "Визелко Александр Сергеевич",
        "Штамов Алексей Александрович",
        "Каратаев Роман Андреевич",
        "Хайрутдинова Алина Эдуардовна",
        "Нуруллин Руслан Алевтинович",
        "Бурнева Анастасия Борисовна",
        "Шифман Антон Эдуардович",
        "Розенков Роберт Дмитриевич",
        "Киселев Максим Сергеевич",
        "Фефелов Василий Владимирович",
        "Сенников Григорий Алексеевич",
        "Лизунов Александр Владимирович",
        "Шкуревских Глеб Константинович",
        "Овчинников Максим Александрович",
        "Шараф Ранда Яссер Мохамед Али",
        "Корнев Андрей Евгеньевич",
        "Ба Амаду Ди Шериф",
        "Хакимов Зафар Джафарович",
        "Вяткина Софья Романовна",
        "Сергеев Алексей Сергеевич",
        "Шандер Саша-Ойген",
        "Орлов Илья Сергеевич",
        "Шмидт Максим Дмитриевич",
        "Заборов Тимофей Андреевич",
        "Каджеметов Егор Вячеславович",
        "Павлов Александр Алексеевич",
        "Данилов Евгений Вячеславович",
        "Сарпов Иван Сергеевич",
        "Бисембаева Анастасия Сергеевна",
        "Селифанова Элина Нажмутдиновна",
        "Кораблев Егор Денисович",
        "Нестеров Дмитрий Денисович",
        "Шокурова Наталья Владимировна",
        "Моисеев Денис Александрович",
        "Снедков Илья Андреевич",
        "Анисимов Виктор Александрович"
    ];
    
    public static string GenerateFullName() => Data[Random.Next(Data.Count)];
}