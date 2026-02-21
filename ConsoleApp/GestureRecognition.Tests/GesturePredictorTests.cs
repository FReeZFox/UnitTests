using Moq;

namespace ConsoleApp.Tests
{
    public class GesturePredictorTests
    {
        /*
            Тест 1: Проверка, что если руки в кадре нет, возвращается no_event
            Ключевые точки хоть и передаются, но т.к. стоит флаг отсутствия рук,
            то модель должна их проигнорировать и выдать no_event (индекс 0) с точностью 1.0.
        */
        [Fact]
        public void Predictor_Test1()
        {
            var mockModel = new Mock<IGestureModel>();
            var predictor = new GesturePredictor(mockModel.Object);

            var frame = new FrameData(false, new float[] { 1f, 2f });
            var result = predictor.Process(frame, Constants.Gestures);

            Assert.Equal(0, result.GestureIndex);
            Assert.Equal(1f, result.Confidence);
            Assert.Single(result.Probabilities);
        }
        
        /*
            Тест 2: Проверка, что если рука в кадре, вызывается Predict 
            и выбирается правильный индекс максимума. Здесь передаются данные так,
            что индекс максимума равен 1.
        */
        [Fact]
        public void Predictor_Test2()
        {
            var mockModel = new Mock<IGestureModel>();
            mockModel.Setup(m => m.Predict(It.IsAny<float[]>())).Returns(new float[] { 0.1f, 0.8f, 0.1f });

            var predictor = new GesturePredictor(mockModel.Object);

            var frame = new FrameData(true, new float[] { 1f, 2f });
            var result = predictor.Process(frame, Constants.Gestures);

            Assert.Equal(1, result.GestureIndex); 
            Assert.Equal(0.8f, result.Confidence);
            Assert.Equal(new float[] { 0.1f, 0.8f, 0.1f }, result.Probabilities);
        }

        /*
            Тест 3: Проверка, что модель не ломается при пустом массиве ключевых точек.
            Неизвестно по каким причинам точки не были извлечены, но должна быть устойчивость.
            В таком случае, модель должна вернуть no_event (индекс 0)
        */
        [Fact]
        public void Predictor_Test3()
        {
            var mockModel = new Mock<IGestureModel>();
            mockModel.Setup(m => m.Predict(It.IsAny<float[]>())).Returns(new float[] { 0.5f, 0.5f });

            var predictor = new GesturePredictor(mockModel.Object);

            var frame = new FrameData(true, Array.Empty<float>());
            var result = predictor.Process(frame, Constants.Gestures);

            Assert.Equal(0, result.GestureIndex); 
            Assert.Equal(0.5f, result.Confidence);
            Assert.Equal(new float[] { 0.5f, 0.5f }, result.Probabilities);
        }
        /*
            Тест 4: Проверка, что отрицательное или нулевое значение ключевых точек не ломает предсказатель.
            Ключевые точки - координаты, которые могут быть нулевыми или отрицательными
        */
        [Fact]
        public void Predictor_Test4()
        {
            var mockModel = new Mock<IGestureModel>();
            mockModel.Setup(m => m.Predict(It.IsAny<float[]>())).Returns(new float[] { 0.2f, 0.8f });

            var predictor = new GesturePredictor(mockModel.Object);

            var frame = new FrameData(true, new float[] { -5f, 0f });
            var result = predictor.Process(frame, Constants.Gestures);

            Assert.Equal(1, result.GestureIndex);
            Assert.Equal(0.8f, result.Confidence);
        }
        
        /*
            Тест 5: Проверка защиты от передачи неизвестных значений.
            В этом тесте модель возвращает вектор вероятностей из 3 значений, хотя
            среди жестов определен только 1, то есть меньше чем вернула модель.
            Этот жест с индексом 0 в векторе вероятностей, имеет точность 0.1 с. Но т.к. 
            предсказатель должен вернуть максимальное значение, которое имеет индекс 1, то модель
            не может определить что это вообще за жест.
        */
        [Fact]
        public void Predictor_Test5()
        {
            var mockModel = new Mock<IGestureModel>();
            mockModel.Setup(m => m.Predict(It.IsAny<float[]>())).Returns(new float[] { 0.1f, 0.8f, 0.1f });
    
            var predictor = new GesturePredictor(mockModel.Object);
            var frame = new FrameData(true, new float[] { 1f, 2f });
            
            var gestureNames = new[] { "А" }; 
            var result = predictor.Process(frame, gestureNames);
            
            Assert.Equal("unknown", result.GestureName);
            Assert.Equal(1, result.GestureIndex); 
        }
    }
}
