namespace ConsoleApp;

/*
    Класс выполняет задачу записи фразы из переданных жестов-букв.
    Класс может записать букву, стереть или поставить пробел. 
    Существует два способа записи: нажатием кнопки или передачи действия
    жестом левой руки.
*/
public class GestureWriter
{
    private readonly List<string> phrase = new();

    /*
        Метод выполняется чаще, когда вызвано записывание действием.
        В текущей реализации запись кнопкой тоже работает этим методом.
    */
    public void ProcessFrame(string gesture, string action)
    {
        if (action == "записать" && gesture != "no_event")
            Write(gesture);

        else if (action == "стереть" && phrase.Count > 0)
            Erase();

        else if (action == "пробел")
            Space();
    }
    
    public void Write(string gesture)
    {
        if (!string.IsNullOrWhiteSpace(gesture) && gesture != "no_event")
            phrase.Add(gesture);
    }

    public void Erase()
    {
        if (phrase.Count > 0)
            phrase.RemoveAt(phrase.Count - 1);
    }

    public void Space()
    {
        phrase.Add(" ");
    }

    public void Clear()
    {
        phrase.Clear();
    }

    public string GetPhrase()
    {
        return string.Concat(phrase);
    }
}