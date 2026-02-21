namespace ConsoleApp;

/*
    Класс формирует предсказатель,
    сюда передаются данные кадра и список меток класса,
    так же для предсказания необходима модель. 
*/
public class GesturePredictor
{
    private readonly IGestureModel model;

    public GesturePredictor(IGestureModel model)
    {
        this.model = model; 
    }

    /*
        Метод формирует процесс обработки одного кадра.
        Проверяет наличие рук в кадре, выполняет предсказание, используя модель,
        а, получив вектор вероятностей, ищет результат с самой высокой точностью
        и полученные результаты передает для формирования вывода.
    */
    public PredictionResult Process(FrameData frame, string[] gestureNames)
    {
        // Когда в кадре нет руки, формируем состояние по умолчанию ("no_event", 1.00)
        if (!frame.HasHand)
            return new PredictionResult(0, 1f, new float[] { 1f }, gestureNames); 

        var probabilities = model.Predict(frame.Keypoints);

        int maxIndex = Array.IndexOf(probabilities, probabilities.Max());
        float confidence = probabilities[maxIndex];

        return new PredictionResult(maxIndex, confidence, probabilities, gestureNames);
    }
}