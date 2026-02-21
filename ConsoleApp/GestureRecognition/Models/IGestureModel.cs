namespace ConsoleApp;

/*
    Интерфейс модели. Если бы модель существовала, то сделав предсказание, 
    она бы вернула вектор вероятностей (результат предсказания)
*/
public interface IGestureModel
{
    float[] Predict(float[] keypoints);  
}