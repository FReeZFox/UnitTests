namespace ConsoleApp;

/*
    Класс формирует данные одного кадра
    в упрощенном варианте ключевые точки рук, на основе которых модель делает предсказание
    и флаг наличия руки в кадре (если руки нет, то и ключевые точки извлечь неоткуда)
*/
public class FrameData
{
    public bool HasHand { get; }          
    public float[] Keypoints { get; }    

    public FrameData(bool hasHand, float[]? keypoints)
    {
        HasHand = hasHand;
        Keypoints = keypoints ?? new float[0]; // Защита от null 
    }
}