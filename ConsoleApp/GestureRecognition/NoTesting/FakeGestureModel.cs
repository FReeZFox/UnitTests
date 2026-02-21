namespace ConsoleApp;

/*
    Данный класс предназначен, чтобы создать заглушку-модель, 
    для работы имитации. Юнит-тесты не распространяются и на этот класс, 
    поэтому он должен быть извлечен из общего процента покрытия кода.
*/
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class FakeGestureModel : IGestureModel
{
    private readonly int gestureCount;
    private readonly Random random = new();

    public FakeGestureModel(int gestureCount)
    {
        if (gestureCount <= 0)
            throw new ArgumentException("gestureCount должен быть > 0");

        this.gestureCount = gestureCount;
    }

    /*
        Метод выполняет фейковое предсказание, случайным образом
        формируя вектор предсказаний.
    */
    public float[] Predict(float[] keypoints)
    {
        var probabilities = new float[gestureCount];
        float sum = 0f;

        // Просто формирует вектор вероятностей
        for (int i = 0; i < gestureCount; i++)
        {
            probabilities[i] = (float)random.NextDouble();
            sum += probabilities[i];
        }
        
        // Имитация нормализации 
        if (sum > 0f)
        {
            for (int i = 0; i < gestureCount; i++)
                probabilities[i] /= sum;
        }
        else
        {
            probabilities[0] = 1f;
        }

        return probabilities;
    }
}