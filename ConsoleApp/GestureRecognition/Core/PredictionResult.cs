namespace ConsoleApp;

/*
    Класс формирует результаты предсказания,
    чтобы вернуть его пользователю. Здесь же, 
    по переданному индексу выясняет имя жеста.
*/
public class PredictionResult
{
    public int GestureIndex { get; }        
    public string GestureName { get; }       
    public float Confidence { get; }         
    public float[] Probabilities { get; }    

    public PredictionResult(int gestureIndex, float confidence, float[]? probabilities, string[] gestureNames)
    {
        GestureIndex = gestureIndex;
        Confidence = confidence;
        Probabilities = probabilities ?? new float[] { 1f };

        // Определяем имя жеста по индексу
        if (gestureIndex >= 0 && gestureIndex < gestureNames.Length)
            GestureName = gestureNames[gestureIndex];
        else
            GestureName = "unknown";
    }
}