namespace ConsoleApp;

/*
    Данный класс предназначен, чтобы вручную посмотреть работу программы, 
    то есть запустить имитацию демонстрации жестов и предсказания.
    Юнит-тесты не распространяются на этот класс, поэтому он должен быть извлечен
    из общего процента покрытия кода. 
*/
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class Program
{
    public static void Main()
    {
        // Своя заглушка модели, которая способна в ответ на ключевые точки возвращать вероятность и жест
        var model = new FakeGestureModel(Constants.Gestures.Length);  
        
        var predictor = new GesturePredictor(model);
        var writer = new GestureWriter();
        var random = new Random();

        // Процесс симуляции из 30 кадров
        for (int frameNumber = 1; frameNumber <= 30; frameNumber++)
        {
            // Определяем в кадре ли руки
            bool hasHand = random.NextDouble() > 0.2;
            
            // Создаем данные кадра (ключевые точки просто заглушка)
            var frame = new FrameData(
                hasHand,
                new float[] { frameNumber, frameNumber + 1, frameNumber + 2 }
            );

            // Делаем предсказание
            var prediction = predictor.Process(frame, Constants.Gestures);

            // Случайно выбираем действие 
            double actionRandom = random.NextDouble();
            
            string action;
            if (actionRandom < 0.5)
                action = "записать";
            else if (actionRandom < 0.7)
                action = "стереть";
            else
                action = "пробел";
            
            writer.ProcessFrame(prediction.GestureName, action);
            Console.WriteLine(
                $"Кадр {frameNumber,2} | Руки в кадре = {hasHand,-5} | Жест = {prediction.GestureName,-8} | Точность = {prediction.Confidence:F2} | " +
                $"Действие = {action,-8}"
            );

            Console.WriteLine($"Текущая фраза: \"{writer.GetPhrase()}\"\n");
        }
    }
}